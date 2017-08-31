using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using FitnessTracker.DTOs;
using FitnessTracker.Models;

namespace FitnessTracker.Utilities
{
    public class NutritionUtil
    {
        private string USDAApiKey = ConfigurationManager.AppSettings["USDAApiKey"];
        private readonly List<string> _nutrientsToTrack;

        public NutritionUtil()
        {
            _nutrientsToTrack = new List<string>()
            {
                "water",
                "energy",
                "protein",
                "lipid",
                "carbohydrate",
                "sugars",
                "calcium",
                "sodium"
            };
        }

        public List<FoodSearchDto> SearchFoodsByName(string searchText, int numResults = 200, string sortBy = "r")
        {
            // API Documentation
            //https://ndb.nal.usda.gov/ndb/doc/apilist/API-SEARCH.md

            List<FoodSearchDto> results = new List<FoodSearchDto>();

            string xml = "";
            string url = @"https://api.nal.usda.gov/ndb/search/?format=xml&q=" + searchText + 
                "&max=" + numResults + "&offset=0&sort=" + sortBy + "&api_key=" + USDAApiKey;
                //@"https://api.nal.usda.gov/ndb/reports/?ndbno=01009&type=b&format=xml&api_key=AAPGMmnC2t1LyDu4pQuBdfZ6CbCwfcpyvolA3ssN";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                xml = reader.ReadToEnd();
            }
            XDocument doc = XDocument.Parse(xml);

            var itemList = doc.Element("list");

            if (itemList != null)
            {
                foreach (var item in itemList.Elements())
                {
                    var foodItem = new FoodSearchDto();
                    foreach (var element in item.Elements())
                    {
                        switch (element.Name.LocalName.ToLower())
                        {
                            case "group":
                                foodItem.Group = element.Value;
                                break;
                            case "name":
                                foodItem.Name = element.Value;
                                break;
                            case "ndbno":
                                foodItem.UsdaDbNum = element.Value;
                                break;
                        }
                    }
                    results.Add(foodItem);
                }
            }

            return results;
        }

        public FoodDetailsDto GetFoodDetails(string usdaDbNum)
        {
            FoodDetailsDto foodDetails = new FoodDetailsDto();

            string xml = "";
            string url = @"https://api.nal.usda.gov/ndb/reports/?ndbno=" + usdaDbNum + "&type=b&format=xml&api_key=" + USDAApiKey;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                xml = reader.ReadToEnd();
            }
            XDocument doc = XDocument.Parse(xml);

            var foodReport = doc.Element("report").Element("food");
            var nutrientsList = foodReport.Element("nutrients");

            foodDetails.Name = foodReport.Attribute("name").Value;
            foodDetails.UsdaDbNum = usdaDbNum;

            List<NutrientDto> nutrients = new List<NutrientDto>();

            foreach (var nutrientElement in nutrientsList.Elements())
            {
                bool trackedNutrient = false;

                foreach (string nutrientToTrack in _nutrientsToTrack)
                {
                    if (nutrientElement.Attribute("name").Value.ToLower().Contains(nutrientToTrack))
                    {
                        trackedNutrient = true;
                    }
                }

                if (!trackedNutrient) { continue; }

                NutrientDto nutrient = new NutrientDto();              
                nutrient.Name = nutrientElement.Attribute("name").Value;
                nutrient.NutrientId = Int32.Parse(nutrientElement.Attribute("nutrient_id").Value);
                nutrient.Unit = nutrientElement.Attribute("unit").Value;
                nutrient.Value = double.Parse(nutrientElement.Attribute("value").Value);

                if (nutrient.Name == "Energy")
                {
                    nutrient.Name += " (Calories)";
                }

                var measuresList = nutrientElement.Element("measures");

                List<MeasureDto> measures = new List<MeasureDto>();

                foreach (var measureElement in measuresList.Elements())
                {
                    MeasureDto measure = new MeasureDto();
                    measure.Label = measureElement.Attribute("label").Value;
                    measure.Equivalent = double.Parse(measureElement.Attribute("eqv").Value);
                    measure.EquivUnit = measureElement.Attribute("eunit").Value;
                    measure.Quantity = double.Parse(measureElement.Attribute("qty").Value);
                    measure.Value = double.Parse(measureElement.Attribute("value").Value);

                    measures.Add(measure);
                }
                nutrient.Measures = measures;
                nutrients.Add(nutrient);
            }

            foodDetails.Nutrients = nutrients;
            return foodDetails;
        }
    }
}
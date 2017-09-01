using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnessTracker.DataAccess;
using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Utilities;
using Microsoft.AspNet.Identity;

namespace FitnessTracker.Controllers
{
    public class NutritionController : Controller
    {
        private NutritionUtil util = new NutritionUtil();
        // GET: Nutrition
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult SearchFoodByName(string searchText)
        {
            var results = util.SearchFoodsByName(searchText);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFoodDetails(string usdaDbNum)
        {
            var foodDetails = util.GetFoodDetails(usdaDbNum);

            return Json(foodDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTodaysNutrientTotals()
        {
            List<FoodItem> foodList = new List<FoodItem>();
            List<Nutrient> nutrientTotals = new List<Nutrient>();

            string todaysDate = DateTime.Today.ToString("yyyyMMdd");
            string username = User.Identity.GetUserName();

            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false;

                foodList = (from foodItem in context.FoodItems
                            where foodItem.Username == username
                                  && foodItem.DateAdded == todaysDate
                            select foodItem).ToList();

                foreach (var foodItem in foodList)
                {
                    List<Nutrient> nutrientsForFood = new List<Nutrient>();

                    nutrientsForFood = (from nutrient in context.Nutrients
                                        where nutrient.FoodId == foodItem.FoodId
                                        select nutrient).ToList();

                    foreach (var nutrient in nutrientsForFood)
                    {
                        var nutrientCheck = nutrientTotals.FirstOrDefault(x => x.Name == nutrient.Name);
                        if (nutrientCheck == null)
                        {
                            Nutrient nutrientToAdd = new Nutrient()
                            {
                                Name = nutrient.Name,
                                FoodId = nutrient.FoodId,
                                Unit = nutrient.Unit,
                                Value = Math.Round(nutrient.Value * nutrient.FoodItem.ServingsConsumed, 2)
                            };
                            nutrientTotals.Add(nutrientToAdd);
                        }
                        else
                        {
                            nutrientCheck.Value += (nutrient.Value * nutrient.FoodItem.ServingsConsumed);
                            nutrientCheck.Value = Math.Round(nutrientCheck.Value, 2);
                        }
                        
                    }
                }
            }

            return Json(nutrientTotals, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTodaysFoods()
        {
            List<FoodItem> foodList = new List<FoodItem>();
            List<Nutrient> nutrientTotals = new List<Nutrient>();

            string todaysDate = DateTime.Today.ToString("yyyyMMdd");
            string username = User.Identity.GetUserName();

            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false;

                foodList = (from foodItem in context.FoodItems
                    where foodItem.Username == username
                          && foodItem.DateAdded == todaysDate
                    select foodItem).ToList();
            }

            return Json(foodList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddFoodItem(FoodDetailsDto foodItemDetails, MeasureDto selectedServingSize, double servings)
        {
            using (var context = new FitnessTrackerContext())
            {
                FoodItem food = new FoodItem();
                string username = User.Identity.GetUserName();
                FitnessUser user = context.FitnessUsers.Find(username);

                food.FitnessUser = user;
                food.Name = foodItemDetails.Name;
                food.UsdaDbNum = foodItemDetails.UsdaDbNum;
                food.DateAdded = DateTime.Today.ToString("yyyyMMdd");

                double servingAmount = selectedServingSize.Quantity;
                string servingLabel = selectedServingSize.Label;
                food.ServingAmount = servingAmount;
                food.ServingLabel = servingLabel;
                food.ServingsConsumed = servings;
                context.FoodItems.Add(food);

                foreach (var nutrient in foodItemDetails.Nutrients)
                {
                    Nutrient nutrientToAdd = new Nutrient();
                    nutrientToAdd.Name = nutrient.Name;
                    nutrientToAdd.Unit = nutrient.Unit;
                    nutrientToAdd.FoodItem = food;

                    double nutrientValue = 0.0;
                    foreach (var measure in nutrient.Measures)
                    {
                        if (food.ServingAmount == measure.Quantity && food.ServingLabel == measure.Label)
                        {
                            nutrientValue = measure.Value;
                            break;
                        }
                    }
                    nutrientToAdd.Value = nutrientValue;
                    context.Nutrients.Add(nutrientToAdd);
                }

                context.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateServingsConsumed(FoodItem foodItem)
        {
            FoodItem foodItemToUpdate;

            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false;

                foodItemToUpdate = context.FoodItems.Find(foodItem.FoodId);

                if (foodItemToUpdate != null)
                {
                    foodItemToUpdate.ServingsConsumed = foodItem.ServingsConsumed;
                }
                context.Entry(foodItemToUpdate).State = System.Data.Entity.EntityState.Modified;

                context.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFoodItem(int foodId)
        {
            FoodItem foodItemToDelete;

            using (var context = new FitnessTrackerContext())
            {
                foodItemToDelete = context.FoodItems.Find(foodId);

                if (foodItemToDelete != null)
                {
                    context.FoodItems.Remove(foodItemToDelete);
                }
                context.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
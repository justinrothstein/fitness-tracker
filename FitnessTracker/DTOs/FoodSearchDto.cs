using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessTracker.Models
{
    public class FoodSearchDto
    {
        public string UsdaDbNum { get; set; } 
        public string Name { get; set; }
        public string Group { get; set; }
        //public int CaloriesPerServing { get; set; }
    }
}
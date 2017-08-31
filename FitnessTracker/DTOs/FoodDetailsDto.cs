using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessTracker.DTOs
{
    public class FoodDetailsDto
    {
        public string UsdaDbNum { get; set; }
        public string Name { get; set; }
        public List<NutrientDto> Nutrients { get; set; }
    }
}
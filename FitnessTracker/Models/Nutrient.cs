using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FitnessTracker.Models
{
    public class Nutrient
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NutrientId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double Value { get; set; }

        [Required]
        [ForeignKey("FoodItem")]
        public int FoodId { get; set; }
        public virtual FoodItem FoodItem { get; set; }
    }
}
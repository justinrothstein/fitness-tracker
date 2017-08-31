using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FitnessTracker.Models
{
    public class FoodItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FoodId { get; set; }

        public string Name { get; set; }
        public string UsdaDbNum { get; set; }
        public string DateAdded { get; set; }
        public double ServingAmount { get; set; }
        public string ServingLabel { get; set; }
        public double ServingsConsumed { get; set; }

        [ForeignKey("FitnessUser")]
        public string Username { get; set; }
        public virtual FitnessUser FitnessUser { get; set; }
    }
}
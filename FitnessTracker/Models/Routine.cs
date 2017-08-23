using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FitnessTracker.Models
{
    public class Routine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoutineID { get; set; }
        [Required]
        public string RoutineName { get; set; }
        public string RoutineGoal { get; set; }
        [Required]
        [ForeignKey("FitnessUser")]
        public string Username { get; set; }
        public bool Active { get; set; }
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }

        public virtual FitnessUser FitnessUser { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;

namespace FitnessTracker.Models
{
    public class Exercise
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExerciseID { get; set; }
        [Required]
        public string ExerciseName { get; set; }
        public string Description { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public int Weight { get; set; }
        [ForeignKey("FitnessUser")]
        public string Username { get; set; }

        public virtual FitnessUser FitnessUser { get; set; }
        public virtual ICollection<Routine> Routines { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FitnessTracker.Models;

namespace FitnessTracker.DataAccess
{
    public class FitnessTrackerContext : DbContext
    {
        public FitnessTrackerContext(): base()
        {
            
        }

        public DbSet<FitnessUser> FitnessUsers { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
    }
}
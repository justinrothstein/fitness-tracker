using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnessTracker.DataAccess;
using FitnessTracker.Models;
using Microsoft.AspNet.Identity;

namespace FitnessTracker.Controllers
{
    public class UserHomeController : Controller
    {
        // GET: UserHome
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetActiveRoutines()
        {
            List<Routine> routines;
            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                string email = User.Identity.GetUserName();
                routines = (from routine in context.Routines
                            where routine.Username == email && routine.Active
                            select routine).ToList();
            }
            return Json(routines, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetTodaysRoutines()
        {
            List<Routine> allRoutines;
            List<Routine> todaysRoutines = new List<Routine>();
            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                string email = User.Identity.GetUserName();
                allRoutines = (from routine in context.Routines
                            where routine.Username == email && routine.Active
                            select routine).ToList();

                string day = DateTime.Today.DayOfWeek.ToString();
                foreach (var routine in allRoutines)
                {
                    switch (day.ToLower())
                    {
                        case "sunday":
                            if (routine.Sunday) todaysRoutines.Add(routine);
                            break;
                        case "monday":
                            if (routine.Monday) todaysRoutines.Add(routine);
                            break;
                        case "tuesday":
                            if (routine.Tuesday) todaysRoutines.Add(routine);
                            break;
                        case "wednesday":
                            if (routine.Wednesday) todaysRoutines.Add(routine);
                            break;
                        case "thursday":
                            if (routine.Thursday) todaysRoutines.Add(routine);
                            break;
                        case "friday":
                            if(routine.Friday) todaysRoutines.Add(routine);
                            break;
                        case "saturday":
                            if(routine.Saturday) todaysRoutines.Add(routine);
                            break;
                    }
                }
            }
            return Json(todaysRoutines, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetRoutineDetails(int routineId)
        {
            List<Exercise> exercises;
            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                exercises = (from exercise in context.Exercises
                             where exercise.Routines.Any(x => x.RoutineID == routineId)
                             select exercise).ToList();
            }

            return Json(exercises, JsonRequestBehavior.AllowGet);
        }
    }
}
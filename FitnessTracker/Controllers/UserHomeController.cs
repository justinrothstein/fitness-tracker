using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnessTracker.DataAccess;
using FitnessTracker.Models;
using FitnessTracker.Utilities;
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


        public void TestApi()
        {
            NutritionUtil util = new NutritionUtil();
            //var results = util.SearchFoodsByName("butter");
            var result = util.GetFoodDetails("21228");
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
        public JsonResult GetTodaysRoutines(string day)
        {
            //TODO maybe need to do on client side for timezone reasons.
            List<Routine> allRoutines;
            List<Routine> todaysRoutines = new List<Routine>();
            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                string email = User.Identity.GetUserName();
                allRoutines = (from routine in context.Routines
                            where routine.Username == email && routine.Active
                            select routine).ToList();
                
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

        [HttpGet]
        public JsonResult GetFirstName()
        {
            string firstName = "";
            string userName = User.Identity.GetUserName();
            using (var context = new FitnessTrackerContext())
            {
                FitnessUser user = (from fitnessUser in context.FitnessUsers
                    where fitnessUser.Username == userName
                    select fitnessUser).FirstOrDefault();
                firstName = user.Firstname;
            }
            return Json(firstName, JsonRequestBehavior.AllowGet);
        }
    }
}
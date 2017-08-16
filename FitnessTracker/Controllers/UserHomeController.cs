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
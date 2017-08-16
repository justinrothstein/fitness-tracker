using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Web;
using System.Web.Mvc;
using FitnessTracker.DataAccess;
using FitnessTracker.Models;
using Microsoft.AspNet.Identity;

namespace FitnessTracker.Controllers
{
    public class FitnessController : Controller
    {
        [Authorize]
        // GET: Fitness
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult CreateRoutine()
        {
            return View();
        }

        [Authorize]
        public ActionResult CreateExercise()
        {
            return View();
        }

        [Authorize]
        public ActionResult EditRoutine()
        {
            return View();
        }

        [Authorize]
        public ActionResult EditExercise()
        {
            return View();
        }

        [Authorize]
        public ActionResult ManageExercises()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateRoutine(string routineName, string routineGoal, string userName, bool isActive, List<Exercise> exercises)
        {
            using (var context = new FitnessTrackerContext())
            {
                var exercisesToAdd = new List<Exercise>();
                foreach (var newExercise in exercises)
                {
                    context.Configuration.LazyLoadingEnabled = false;
                    Exercise ex = context.Exercises.FirstOrDefault(x => x.ExerciseID == newExercise.ExerciseID);

                    if (ex != null)
                    {
                        exercisesToAdd.Add(ex);
                    }
                    else
                    {
                        ex = new Exercise()
                        {
                            ExerciseName = newExercise.ExerciseName,
                            Description = newExercise.Description,
                            Sets = newExercise.Sets,
                            Username = userName,
                            Reps = newExercise.Reps
                        };
                        exercisesToAdd.Add(ex);
                    }
                }

                var routine = new Routine() {RoutineName = routineName, RoutineGoal = routineGoal, Username = userName, Active = isActive, Exercises = exercisesToAdd };
                context.Routines.Add(routine);
                context.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateExercise(string exerciseName, string description, int sets, int reps, string userName)
        {
            using (var context = new FitnessTrackerContext())
            {
                var exercise = new Exercise() {ExerciseName = exerciseName, Description = description, Sets = sets, Reps = reps, Username = userName};
                context.Exercises.Add(exercise);
                context.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateRoutine(Routine routine, List<Exercise> exercises)
        {
            Routine routineToUpdate;
            var exercisesToUpdate = new List<Exercise>();
            using (var context = new FitnessTrackerContext())
            {
                routineToUpdate = context.Routines.Find(routine.RoutineID);
                foreach (var newExercise in exercises)
                {
                    context.Configuration.LazyLoadingEnabled = false;
                    Exercise ex = context.Exercises.FirstOrDefault(x => x.ExerciseID == newExercise.ExerciseID);

                    if (ex != null)
                    {
                        exercisesToUpdate.Add(ex);
                    }
                    else
                    {
                        ex = new Exercise()
                        {
                            ExerciseName = newExercise.ExerciseName,
                            Description = newExercise.Description,
                            Sets = newExercise.Sets,
                            Username = User.Identity.GetUserName(),
                            Reps = newExercise.Reps
                        };
                        exercisesToUpdate.Add(ex);
                    }
                }


                if (routineToUpdate != null)
                {
                    routineToUpdate.RoutineName = routine.RoutineName;
                    routineToUpdate.RoutineGoal = routine.RoutineGoal;
                    routineToUpdate.Active = routine.Active;
                }


                context.Entry(routineToUpdate).Collection("Exercises").Load();

                for (int i = 0; i < routineToUpdate.Exercises.Count; i++)
                {
                    if (!exercisesToUpdate.Contains(routineToUpdate.Exercises.ElementAt(i)))
                    {
                        routineToUpdate.Exercises.Remove(routineToUpdate.Exercises.ElementAt(i));
                        i--;
                    }
                }

                foreach (var exercise in exercisesToUpdate)
                {
                    if (!routineToUpdate.Exercises.Contains(exercise))
                    {
                        routineToUpdate.Exercises.Add(exercise);
                    }
                }

                context.Entry(routineToUpdate).State = System.Data.Entity.EntityState.Modified;

                context.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateExercise(Exercise exercise)
        {
            Exercise exerciseToUpdate;

            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false; 

                exerciseToUpdate = context.Exercises.Find(exercise.ExerciseID);

                if (exerciseToUpdate != null)
                {
                    exerciseToUpdate.ExerciseName = exercise.ExerciseName;
                    exerciseToUpdate.Description = exercise.Description;
                    exerciseToUpdate.Sets = exercise.Sets;
                    exerciseToUpdate.Reps = exercise.Reps;
                }

                context.Entry(exerciseToUpdate).State = System.Data.Entity.EntityState.Modified;

                context.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllRoutines()
        {
            List<Routine> routines;
            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                string email = User.Identity.GetUserName();
                routines = (from routine in context.Routines
                            where routine.Username == email
                            select routine).ToList();
            }
            return Json(routines, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetRoutine(int routineId)
        {
            Routine routine;
            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                routine = (from routines in context.Routines
                            where routines.RoutineID == routineId
                            select routines).First();
            }
            return Json(routine, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetExercises(int routineId)
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
        public JsonResult GetExistingExercises()
        {
            List<Exercise> exercises;
            string username = User.Identity.GetUserName();

            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                exercises = (from exercise in context.Exercises
                             where exercise.Username == username
                             select exercise).ToList();
            }

            return Json(exercises, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetExercise(int exerciseId)
        {
            Exercise exercise;

            using (var context = new FitnessTrackerContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                exercise = (from exercises in context.Exercises
                    where exercises.ExerciseID == exerciseId
                    select exercises).First();
            }

            return Json(exercise, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteRoutine(int routineId)
        {
            Routine routineToDelete;

            using (var context = new FitnessTrackerContext())
            {
                routineToDelete = context.Routines.Find(routineId);

                if (routineToDelete != null)
                {
                    context.Routines.Remove(routineToDelete);
                }
                context.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteExercise(int exerciseId)
        {
            Exercise exerciseToDelete;

            using (var context = new FitnessTrackerContext())
            {
                exerciseToDelete = context.Exercises.Find(exerciseId);

                if (exerciseToDelete != null)
                {
                    context.Exercises.Remove(exerciseToDelete);
                }

                context.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
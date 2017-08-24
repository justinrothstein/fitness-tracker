var app = angular.module("fitnessModule", []);

app.controller("fitnessCreateController",
    function($scope, $http, $location, $window) {
        var model = {};

        model.routineName = "";
        model.routineGoal = "";
        model.isRoutineActive = false;
        model.days = { Sunday: false, Monday: false, Tuesday:false, Wednesday: false, Thursday: false, Friday: false, Saturday: false};

        model.exerciseName = "";
        model.exerciseDescription = "";
        model.exerciseSets = "";
        model.exerciseReps = "";
        model.exerciseWeight = "";

        model.exercises = [];
        model.existingExercises = [];

        model.existingExerciseSelected = null;

        $scope.model = model;

        loadExistingExercises();

    // TODO Put in common service?
    $scope.changeView = function (view) {
        $window.location.href = basePath + view;
    };

    $scope.createRoutine = function (userName) {
        createNewRoutine(userName);
    };

    $scope.createExercise = function (userName) {
        createNewExercise(userName);
    };

    $scope.addNewExercise = function() {

        var newIndex = model.exercises.length + 1;
        var newExercise = {
            ExerciseName: "",
            Description: "",
            Sets: "",
            Reps: ""
        };

        model.exercises.splice(newIndex, 0, newExercise);
    }

        $scope.addExistingExercise = function() {
            var newIndex = model.exercises.length + 1;
            var newExercise = model.existingExerciseSelected;

            newExercise.Existing = true;

            model.exercises.splice(newIndex, 0, newExercise);

            var indexOfExisting = model.existingExercises.indexOf(newExercise);
            model.existingExercises.splice(indexOfExisting, 1);

            model.existingExerciseSelected = null;
        }

    $scope.removeExercise = function(exercise) {
        var index = model.exercises.indexOf(exercise);
        model.exercises.splice(index, 1);

        var newIndex = model.existingExercises.length + 1;
        model.existingExercises.splice(newIndex, 0, exercise);
    }

    //---------------------------------------

    function createNewRoutine(userName) {
        var url = basePath + "Fitness/CreateRoutine";
        var postParams = {
            routineName: model.routineName,
            routineGoal: model.routineGoal,
            userName: userName,
            isActive: model.isRoutineActive,
            exercises: model.exercises,
            sunday: model.days.Sunday,
            monday: model.days.Monday,
            tuesday: model.days.Tuesday,
            wednesday: model.days.Wednesday,
            thursday: model.days.Thursday,
            friday: model.days.Friday,
            saturday: model.days.Saturday
        };

        $http.post(url, postParams)
        .then(function (result) {
            if (result.data === true) {
                $window.location.href = basePath + "Fitness/Index";
            }
        });
    }

    function createNewExercise(userName) {
        var url = basePath + "Fitness/CreateExercise";
        var postParams = {
            exerciseName: model.exerciseName,
            description: model.exerciseDescription,            
            sets: model.exerciseSets,
            reps: model.exerciseReps,
            weight: model.exerciseWeight,
            userName: userName
        };

        $http.post(url, postParams)
        .then(function (result) {
            if (result.data === true) {
                $window.location.href = basePath + "Fitness/Index";
            }
        });
    }

        function loadExistingExercises() {
            var url = basePath + "Fitness/GetExistingExercises";
            model.existingExercises = [];

            $http.get(url)
                .then(function(result) {
                    model.existingExercises = result.data;
                });
        }

});
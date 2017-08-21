var app = angular.module("fitnessModule", []);

app.controller("fitnessEditController", function ($scope, $http, $location, $window) {
    var model = {};

    model.routineToEdit = {};

    model.existingExercises = [];
    model.existingExerciseSelected = null;

    $scope.model = model;

    getRoutineToEdit($window.location.search.split('=')[1], loadExistingExercises);   

    // TODO Put in common service?
    $scope.changeView = function (view) {
        $window.location.href = basePath + view;
    };

    $scope.removeExercise = function (exercise) {
        var index = model.routineToEdit.exercises.indexOf(exercise);
        model.routineToEdit.exercises.splice(index, 1);

        var newIndex = model.existingExercises.length + 1;
        model.existingExercises.splice(newIndex, 0, exercise);
    }

    $scope.addNewExercise = function () {

        var newIndex = model.routineToEdit.exercises.length + 1;
        var newExercise = {
            ExerciseName: "",
            Description: "",
            Sets: "",
            Reps: "",
            Weight: ""
        };

        model.routineToEdit.exercises.splice(newIndex, 0, newExercise);
    }

    $scope.addExistingExercise = function () {
        var newIndex = model.routineToEdit.exercises.length + 1;
        var newExercise = model.existingExerciseSelected;

        model.routineToEdit.exercises.splice(newIndex, 0, newExercise);

        var indexOfExisting = model.existingExercises.indexOf(newExercise);
        model.existingExercises.splice(indexOfExisting, 1);

        model.existingExerciseSelected = null;
    }

    $scope.saveChangesClicked = function() {
        updateRoutine();
    }

    //---------------------------------------

    function getRoutineToEdit(routineId, callback) {
        model.routineToEdit = {};
        var url = basePath + "Fitness/GetRoutine?routineId=" + routineId;

        $http.get(url)
            .then(function(result) {
                model.routineToEdit = result.data;
                getExercisesForRoutine(model.routineToEdit, callback);
            });
    }

    function getExercisesForRoutine(routine, callback) {
        routine.exercises = [];

        var url = basePath + "UserHome/GetRoutineDetails" + "?routineId=" + routine.RoutineID;

        $http.get(url)
            .then(function (result) {
                routine.exercises = result.data;
            }).then(function() {
                if (callback) {
                    callback(routine);
                }
            });
    }

    function loadExistingExercises(routine) {
        var url = basePath + "Fitness/GetExistingExercises";
        model.existingExercises = [];

        $http.get(url)
            .then(function (result) {
                model.existingExercises = result.data;

                for (var i = 0; i < routine.exercises.length; i++) {
                    for (var j = 0; j < model.existingExercises.length; j++) {
                        if (model.existingExercises[j].ExerciseID == routine.exercises[i].ExerciseID) {
                            model.existingExercises.splice(j, 1);
                            break;
                        }
                    }
                }
            });
    }

    function updateRoutine() {
        var url = basePath + "Fitness/UpdateRoutine";
        var params =
        {
            routine : model.routineToEdit,
            exercises : model.routineToEdit.exercises
        }

        $http.post(url, params)
            .then(function(result) {
                if (result.data === true) {
                    $window.location.href = basePath + "Fitness/Index";
                }
            });
    }

});
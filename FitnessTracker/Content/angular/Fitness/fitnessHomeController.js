var app = angular.module("fitnessModule", []);

app.controller("fitnessHomeController", function ($scope, $http, $location, $window, notification) {
    var model = {};

    model.routineName = "";
    model.routineGoal = "";
    model.isRoutineActive = false;
    model.allRoutines = [];
    model.routineToEdit = {};

    model.routineToDelete = {};

    $scope.model = model;


    //getRoutineToEdit($window.location.search.split('=')[1]);

    getAllRoutines();


    // TODO Put in common service?
    $scope.changeView = function (view) {
        $window.location.href = basePath + view;
    };

    $scope.createRoutine = function(userName) {
        createNewRoutine(userName);
    };

    $scope.expandRoutineClicked = function(routine) {
        routine.routineExpanded = true;
        getExercisesForRoutine(routine);
    };

    $scope.collapseRoutineClicked = function (routine) {
        routine.routineExpanded = false;
        routine.exercises = [];
    };

    $scope.editRoutine = function(routine) {
        model.routineToEdit = routine;
        $window.location.href = basePath + "Fitness/EditRoutine?routineId=" + routine.RoutineID;
    }

    $scope.editExercise = function (exercise) {
        $window.location.href = basePath + "Fitness/EditExercise?exerciseId=" + exercise.ExerciseID;
    }

    $scope.deleteRoutineClicked = function (routine) {
        model.routineToDelete = routine;
        showConfirmDeleteRoutineModal(); 
    }

    $scope.confirmDeleteClicked = function() {
        deleteRoutine(model.routineToDelete);
    }

    //---------------------------------------

    function showConfirmDeleteRoutineModal() {
        $('#confirmDeleteRoutineWindow').modal('show');
    }

    function createNewRoutine(userName) {
        var url = basePath + "CreateRoutine";
        var postParams = {
            routineName: model.routineName,
            routineGoal: model.routineGoal,
            userName: userName,
            isActive: model.isRoutineActive
        };

        $http.post(url, postParams)
        .then(function(result) {
                if (result.data === true) {
                    $window.location.href = basePath + "UserHome/Index";
                }
            });
    }

    function getAllRoutines() {
        model.allRoutines = [];
        var url = basePath + "Fitness/GetAllRoutines";

        $http.get(url)
            .then(function (result) {
                model.allRoutines = result.data;

                angular.forEach(model.allRoutines,
                    function(value) {
                        value.routineExpanded = false;
                    });
            });
    }

    function deleteRoutine(routine) {
        var url = basePath + "Fitness/DeleteRoutine";

        var params = {
            routineId: routine.RoutineID
        };

        $http.post(url, params)
            .then(function (result) {
                if (result.status == '200') {
                    notification.success("Deleted routine.");
                }
                else {
                    notification.error("Problem deleting routine.");
                }
            }).then(function () {
                model.routineToDelete = {};
                getAllRoutines();
            });

    }

    //function getRoutineToEdit(routineId) {
    //    model.routineToEdit = {};
    //    var url = basePath + "Fitness/GetRoutine?routineId=" + routineId;

    //    $http.get(url)
    //        .then(function (result) {
    //            model.routineToEdit = result.data;
    //            getExercisesForRoutine(model.routineToEdit);
    //        });
    //}

    function getExercisesForRoutine(routine) {
        routine.exercises = [];

        var url = basePath + "UserHome/GetRoutineDetails" + "?routineId=" + routine.RoutineID;

        $http.get(url)
            .then(function (result) {
                routine.exercises = result.data;
            });
    }
});
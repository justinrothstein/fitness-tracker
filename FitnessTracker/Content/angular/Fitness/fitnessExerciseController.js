var app = angular.module("fitnessModule", []);

app.controller("fitnessExerciseController", function ($scope, $http, $location, $window, notification) {
    var model = {};

    model.allExercises = [];
    model.exerciseToDelete = {};

    $scope.model = model;

    getAllExercises();


    // TODO Put in common service?
    $scope.changeView = function (view) {
        $window.location.href = basePath + view;
    };

    //$scope.createExercise = function (userName) {
    //    createNewRoutine(userName);
    //};

    $scope.editExercise = function (exercise) {
        $window.location.href = basePath + "Fitness/EditExercise?exerciseId=" + exercise.ExerciseID;
    }

    $scope.deleteExerciseClicked = function (exercise) {
        model.exerciseToDelete = exercise;
        showConfirmDeleteExerciseModal();
    }

    $scope.confirmDeleteClicked = function () {
        deleteExercise(model.exerciseToDelete);
    }

    //---------------------------------------

    function showConfirmDeleteExerciseModal() {
        $('#confirmDeleteExerciseWindow').modal('show');
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
        .then(function (result) {
            if (result.data === true) {
                $window.location.href = basePath + "UserHome/Index";
            }
        });
    }

    function getAllExercises() {
        model.allExercises = [];
        var url = basePath + "Fitness/GetExistingExercises";

        $http.get(url)
            .then(function (result) {
                model.allExercises = result.data;
            });
    }

    function deleteExercise(exercise) {
        var url = basePath + "Fitness/DeleteExercise";

        var params = {
            exerciseId: exercise.ExerciseID
        };

        $http.post(url, params)
            .then(function (result) {
                if (result.status == '200') {
                    notification.success("Deleted exercise.");
                }
                else {
                    notification.error("Problem deleting exercise.");
                }
            }).then(function () {
                model.exerciseToDelete = {};
                getAllExercises();
            });

    }

});
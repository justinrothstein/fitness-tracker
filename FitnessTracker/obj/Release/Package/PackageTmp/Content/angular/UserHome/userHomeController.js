var app = angular.module("userHomeModule", []);

//myApp.config(function ($routeProvider) {

//    //TODO_AddingNewPanel:  Add form here

//    // Standard Info
//    $routeProvider.when("/createRoutine", {
//        controller: "routineController",
//        templateUrl: function () {
//            return "/UserHome/CreateRoutine";
//        },
//        caseInsensitiveMatch: true
//    });

//    $routeProvider.otherwise({ redirectTo: "/UserHome/Index" });
//});

app.controller("userHomeController", function ($scope, $http, $location, $window) {
    var model = {};
    model.routines = [];
    model.exercisesForRoutine = [];
    model.selectedRoutineID = 0;

    $scope.model = model;

    getActiveRoutines();

    $scope.changeView = function (view) {
        $window.location.href = basePath + view;
    };

    $scope.detailsClick = function (routineId) {
        model.selectedRoutineID = routineId;
        routineDetails(routineId);
    }

    $scope.hideDetailsClick = function () {
        model.selectedRoutineID = 0;
        model.exercisesForRoutine = [];
    }

    function getActiveRoutines() {
        model.routines = [];
        var url = basePath + "UserHome/GetActiveRoutines";

        $http.get(url)
            .then(function (result) {
                model.routines = result.data;
            });
    }

    function routineDetails(routineId) {
        model.exercisesForRoutine = [];

        var url = basePath + "UserHome/GetRoutineDetails" + "?routineId=" + routineId;

        $http.get(url)
            .then(function (result) {
                model.exercisesForRoutine = result.data;
            });
    }

});
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
    model.activeRoutines = [];
    model.exercisesForRoutine = [];
    model.selectedRoutineID = 0;

    model.todaysRoutines = [];

    $scope.model = model;

    getActiveRoutines();
    getTodaysRoutines();

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
        model.activeRoutines = [];
        var url = basePath + "UserHome/GetActiveRoutines";

        $http.get(url)
            .then(function (result) {
                model.activeRoutines = result.data;
            });
    }

    function getTodaysRoutines() {
        model.todaysRoutines = [];
        var url = basePath + "UserHome/GetTodaysRoutines";

        $http.get(url)
            .then(function(result) {
                model.todaysRoutines = result.data;
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
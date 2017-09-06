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

    model.userFirstName = '';

    model.todaysRoutines = [];
    model.todaysNutrientTotals = [];

    $scope.model = model;

    getActiveRoutines();
    getTodaysRoutines();
    getFirstName();
    getTodaysNutrientTotals();

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
        var date = new Date();
        var day = moment(date).format('dddd');

        model.todaysRoutines = [];
        var url = basePath + "UserHome/GetTodaysRoutines?day=" + day;

        $http.get(url)
            .then(function(result) {
                model.todaysRoutines = result.data;
            });
    }

    function getTodaysNutrientTotals() {
        var date = new Date();
        var formattedDate = moment(date).format('YYYYMMDD');

        var url = basePath + "Nutrition/GetTodaysNutrientTotals?date=" + formattedDate;

        $http.get(url)
            .then(function (result) {
                model.todaysNutrientTotals = result.data;
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

    function getFirstName() {
        model.userFirstName = '';

        var url = basePath + "UserHome/GetFirstName";

        $http.get(url)
            .then(function(result) {
                model.userFirstName = result.data;
            });
    }

});
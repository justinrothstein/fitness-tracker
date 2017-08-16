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
    $scope.products = ["Milk", "Bread", "Cheese"];
    var model = {};
    model.routines = [];
    model.exercisesForRoutine = [];

    $scope.model = model;



    getActiveRoutines();

    $scope.changeView = function (view) {
        $window.location.href = basePath + view;
    };

    $scope.detailsClick = function(routineId) {
        routineDetails(routineId);
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
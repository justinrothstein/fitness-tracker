var app = angular.module("fitnessModule", []);

app.controller("fitnessEditExerciseController", function ($scope, $http, $location, $window) {
    var model = {};

    model.exerciseToEdit = {};

    $scope.model = model;

    getExerciseToEdit($window.location.search.split('=')[1]);   

    // TODO Put in common service?
    $scope.changeView = function (view) {
        $window.location.href = basePath + view;
    };

    $scope.saveChangesClicked = function() {
        updateExercise();
    }

    //---------------------------------------

    function getExerciseToEdit(exerciseId) {
        model.exerciseToEdit = {};
        var url = basePath + "Fitness/GetExercise?exerciseId=" + exerciseId;

        $http.get(url)
            .then(function(result) {
                model.exerciseToEdit = result.data;
            });
    }

    function updateExercise() {
        var url = basePath + "Fitness/UpdateExercise";
        var params =
        {
            exercise : model.exerciseToEdit
        }

        $http.post(url, params)
            .then(function(result) {
                if (result.data === true) {
                    $window.location.href = basePath + "Fitness/ManageExercises";
                }
            });
    }

});
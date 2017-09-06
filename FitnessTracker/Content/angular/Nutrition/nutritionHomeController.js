var app = angular.module("nutritionModule", []);

app.controller("nutritionHomeController", function ($scope, $http, $location, $window, notification) {
    var model = {};

    model.searchText = "";

    model.searchList = [];

    model.foodDetails = {};

    model.foodNutrientMeasures = [];
    model.selectedServingSize = "";
    model.servings = "";

    model.recordStart = "";
    model.recordEnd = "";
    model.recordViewAmount = 5;

    model.todaysFoods = [];
    model.todaysNutrientTotals = [];

    $scope.model = model;

    getTodaysFoods();
    getTodaysNutrientTotals();

    // TODO Put in common service?
    $scope.changeView = function (view) {
        $window.location.href = basePath + view;
    };

    $scope.searchButtonClicked = function () {
        model.searchList = [];
        getSearchResults(model.searchText);
    };

    $scope.clearSearchButtonClicked = function () {
        model.searchText = "";
        model.searchList = [];
        model.foodDetails = {};
        model.foodNutrientMeasures = [];
        model.selectedServingSize = "";
        model.servings = "";
        model.recordStart = "";
        model.recordEnd = "";
    }

    $scope.foodItemClicked = function(foodItem) {
        model.foodDetails = {};
        model.foodNutrientMeasures = [];
        model.selectedServingSize = {};
        model.servings = "";
        getFoodDetails(foodItem);
        showFoodDetailsModal();
    };

    $scope.onServingSizeChanged = function() {
        updateNutritionDetailsForServingSize();
    }

    $scope.onPrevResultsClick = function () {
        model.recordEnd = model.recordStart - 1;
        if ((model.recordStart - model.recordViewAmount) < 0) {
            model.recordStart = 0;
        } else {
            model.recordStart -= model.recordViewAmount;
        }
    };

    $scope.onNextResultsClick = function () {
        model.recordStart = model.recordEnd + 1;
        if ((model.searchList.length - (model.recordEnd + 1)) < model.recordViewAmount) {
            model.recordEnd = model.searchList.length - 1;
        } else {
            model.recordEnd += model.recordViewAmount;
        }
    };

    $scope.onAddFoodClick = function() {
        addFoodItem(model.foodDetails);
    }

    $scope.deleteFoodItemClicked = function(foodItem) {
        deleteFoodItem(foodItem);
    }

    $scope.increaseServingClicked = function(foodItem) {
        foodItem.ServingsConsumed += .5;
        updateServingsConsumed(foodItem);
    }

    $scope.decreaseServingClicked = function (foodItem) {
        foodItem.ServingsConsumed -= .5;
        updateServingsConsumed(foodItem);
    }

    //------------------------------------------------

    function showFoodDetailsModal() {
        $('#foodDetailsWindow').modal('show');
    }

    function hideFoodDetailsModal() {
        $('#foodDetailsWindow').modal('toggle');
    }

    function updateNutritionDetailsForServingSize() {
        angular.forEach(model.foodDetails.Nutrients,
            function (nutrient) {
                angular.forEach(nutrient.Measures,
                    function (measure) {
                        if (measure.Label === model.selectedServingSize.Label && measure.Quantity === model.selectedServingSize.Quantity) {
                            nutrient.currentServingSize = measure;
                        }
                    });
            });
    }

    function getSearchResults(searchText) {
        var url = basePath + "Nutrition/SearchFoodByName?searchText=" + searchText;

        model.recordStart = "";
        model.recordEnd = "";

        $http.get(url)
            .then(function(result) {
                model.searchList = result.data;

                if (model.searchList.length === 0) {
                    notification.warning("No Results Found");
                }

                model.recordStart = 0;
                if (model.searchList.length < model.recordViewAmount) {
                    model.recordEnd = model.searchList.length - 1;
                } else {
                    model.recordEnd = model.recordViewAmount - 1;
                }
            });
    }

    function getFoodDetails(foodItem) {
        var url = basePath + "Nutrition/GetFoodDetails?usdaDbNum=" + foodItem.UsdaDbNum;

        //$.blockUI({ message: 'Getting food details...', baseZ: 4000 });

        $http.get(url)
            .then(function(result) {
                model.foodDetails = result.data;
                model.foodNutrientMeasures = model.foodDetails.Nutrients[0].Measures;
                model.selectedServingSize = model.foodDetails.Nutrients[0].Measures[0];

                updateNutritionDetailsForServingSize();

            }).then(function () {
                // Finally block
                //$.unblockUI();
            });
    }

    function getTodaysFoods() {
        var date = new Date();
        var formattedDate = moment(date).format('YYYYMMDD');

        var url = basePath + "Nutrition/GetTodaysFoods?date=" + formattedDate;

        $http.get(url)
            .then(function (result) {
                model.todaysFoods = result.data;
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

    function addFoodItem(foodItem) {
        var date = new Date();
        var formattedDate = moment(date).format('YYYYMMDD');

        var url = basePath + "Nutrition/AddFoodItem";
        var postParams = {
            foodItemDetails: foodItem,
            selectedServingSize: model.selectedServingSize,
            servings: model.servings,
            date: formattedDate
        };

        $http.post(url, postParams)
        .then(function (result) {
            if (result.data === true) {
                hideFoodDetailsModal();
                
            }
        }).then(function () {
            // Finally block
            getTodaysFoods();
            getTodaysNutrientTotals();
            notification.success("Food Item added!");
        });;
    }

    function updateServingsConsumed(foodItem) {
        var url = basePath + "Nutrition/UpdateServingsConsumed";
        var postParams = {
            foodItem: foodItem
        };

        $http.post(url, postParams)
        .then(function (result) {
            if (result.data === true) {
                //Nothing to do?
            }
        }).then(function () {
            // Finally block
            //getTodaysFoods(); Not needed?
            getTodaysNutrientTotals();
        });
    }

    function deleteFoodItem(foodItem) {
        var url = basePath + "Nutrition/DeleteFoodItem";
        var postParams = {
            foodId: foodItem.FoodId
        };

        $http.post(url, postParams)
        .then(function (result) {
            if (result.data === true) {
                notification.success("Food Item Deleted.");
            }
        }).then(function () {
            // Finally block
            getTodaysFoods();
            getTodaysNutrientTotals();
        },
                function (result) {
                    // Error
                    if (result.status == '500') {
                        // 404 errors will occur if user moves off page before AJAX is finished. 
                        notification.error("Error deleting food item.");
                    }
                });
    }

});

// This service is used to notify the user. Uses toastr.js.
app.factory('notification', function () {
    return {
        success: function (message) {
            // Success
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "positionClass": "toast-top-left",
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "4000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }

            toastr.success(message);
        },
        warning: function (message) {
            // Error
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "positionClass": "toast-top-full-width",
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }

            toastr.warning(message);
        },
        error: function (message) {
            // Error
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "positionClass": "toast-top-full-width",
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }

            toastr.error(message);
        }
    };
});
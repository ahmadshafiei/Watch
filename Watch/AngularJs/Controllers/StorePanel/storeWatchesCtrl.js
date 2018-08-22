var app = angular.module('watch');

app.controller('storeWatchesCtrl', function ($scope, sellerService, ngDialog, toaster, NgMap) {

    $scope.pagination = {
        pageSize: 10,
        maxSize: 5,
        pageNumber: 1,
        count: null,
        loading: false,
        searchExp: '',
        pageChanged: function () {
            getAllStoreWatches();
        }
    };

    $scope.init = function () {
        getAllStoreWatches();
    }

    $scope.search = function () {
        $scope.pagination.pageNumber = 1;
        getAllStoreWatches();
    }

    $scope.keySearch = function (e) {
        if (e.keyCode == 13)
            $scope.search();
    }

    function getAllStoreWatches() {
        Pace.start();
        $scope.pagination.loading = true;
        storeService.getAllStoresWatches($scope.pagination.pageNumber, $scope.pagination.pageSize, $scope.pagination.searchExp).then(function (response) {
            $scope.storeWatches = response.data.Result.Data;
            $scope.pagination.count = response.data.Result.Count;
            $scope.pagination.loading = true;
            Pace.stop();
        }, function (response) {
            $scope.pagination.loading = true;
            Pace.stop();
        });
    }

});
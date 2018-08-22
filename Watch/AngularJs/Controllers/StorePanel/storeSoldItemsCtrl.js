var app = angular.module('watch');

app.controller('storeProfileCtrl', function ($scope, sellerService, ngDialog, toaster, NgMap) {

    $scope.pagination = {
        pageSize: 10,
        maxSize: 5,
        pageNumber: 1,
        count: null,
        loading: false,
        searchExp: '',
        pageChanged: function () {
            getAllStoreSoldItems();
        }
    };

    $scope.init = function () {
        getAllStoreSoldItems();
    }

    $scope.search = function () {
        $scope.pagination.pageNumber = 1;
        getAllStoreSoldItems();
    }

    $scope.keySearch = function (e) {
        if (e.keyCode == 13)
            $scope.search();
    }

    function getAllStoreSoldItems() {
        Pace.start();
        $scope.pagination.loading = true;
        storeService.getAllStoresSoldItems($scope.pagination.pageNumber, $scope.pagination.pageSize, $scope.pagination.searchExp).then(function (response) {
            $scope.storeSoldItems = response.data.Result.Data;
            $scope.pagination.count = response.data.Result.Count;
            $scope.pagination.loading = true;
            Pace.stop();
        }, function (response) {
            $scope.pagination.loading = true;
            Pace.stop();
        });
    }

});
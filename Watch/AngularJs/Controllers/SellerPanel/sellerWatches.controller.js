var app = angular.module('watch');

app.controller('sellerWatchesController', function ($scope, $cookies, ngDialog, sellerService) {

    $scope.watchesPagination = {
        pageSize: 10,
        maxSize: 5,
        pageNumber: 1,
        count: null,
        loading: false,
        searchExp: '',
        pageChanged: function () {
            getStoreWatches();
        }
    };

    $scope.init = function () {
        getStoreWatches();
    }

    $scope.openWatchDetailDialog = function () {
        ngDialog.open({
            template: '/Statics/SellerPanel/watchDetail.html',
            className: 'ngdialog-theme-default',
            //width: '50%',
            //height: '50%',
            scope: $scope,
            showClose: true, 
        });
    }

    function getStoreWatches() {
        $scope.watchesPagination.loading = true;
        sellerService.getStoreWatches({ pageNumber: $scope.watchesPagination.pageNumber, pageSize: $scope.watchesPagination.pageSize, searchExp: $scope.watchesPagination.searchExp }).then(function (response) {
            $scope.watches = response.data.Result.Data;
            $scope.watchesPagination.count = response.data.Result.Count;
            $scope.watchesPagination.loading = false;
        }, function (error) {
            $scope.watchesPagination.loading = false;
        });
    }

});
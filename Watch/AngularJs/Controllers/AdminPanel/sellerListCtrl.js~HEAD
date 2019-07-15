var app = angular.module('watch');

app.controller('sellerListCtrl', function ($scope, sellerService, ngDialog, toaster, NgMap) {

    $scope.result = {
        success: null,
        message: '',
    }

    $scope.pagination = {
        pageSize: 6,
        maxSize: 5,
        pageNumber: 1,
        count: null,
        loading: false,
        searchExp: '',
        pageChanged: function () {
            getAllStores();
        }
    };

    $scope.init = function () {
        getAllStores();
    }

    $scope.search = function () {
        $scope.pagination.pageNumber = 1;
        getAllStores();
    }

    $scope.keySearch = function (e) {
        if (e.keyCode == 13)
            $scope.search();
    }

    $scope.removeStore = function (id) {
        $scope.removeStoreId = id;
        ngDialog.open({
            template: '/Statics/AdminPanel/removeConfirmationPartial.html',
            className: 'ngdialog-theme-default',
            width: '50%',
            height: '50%',
            scope: $scope,
            showClose: true,
            controller: ['sellerService', 'toaster', function (sellerService, toaster) {
                $scope.removeStoreConfirmed = function () {
                    sellerService.removeStore($scope.removeStoreId).then(function (response) {
                        toaster.pop('info', 'فروشگاه مورد نظر حذف گردید');
                        ngDialog.closeAll();
                        getAllStores();
                    }, function (response) {
                        toaster.pop('error', 'خطا در برقراری ارتباط با سرور');
                        ngDialog.closeAll();
                    });
                }

            }]
        });
    }

    $scope.showLocationOnMap = function (lat, lng) {
        $scope.storeLatitude = lat;
        $scope.storeLongitude = lng;
        ngDialog.open({
            template: '/Statics/storeLocationOnMap.html',
            className: 'ngdialog-theme-default',
            width: '800px',
            height: '500px',
            scope: $scope,
            showClose: true,
            controller: ['NgMap', function (NgMap) {
                NgMap.getMap().then(function (map) {
                    $scope.map = map;
                    map.setZoom(13);
                });
            }]
        });
    }

    function getAllStores() {
        Pace.start();
        $scope.pagination.loading = true;
        sellerService.getAllStores($scope.pagination.pageNumber, $scope.pagination.pageSize, $scope.pagination.searchExp).then(function (response) {
            $scope.stores = response.data.Result.Data;
            $scope.pagination.count = response.data.Result.Count;
            $scope.pagination.loading = true;
            Pace.stop();
        }, function (response) {
            $scope.pagination.loading = true;
            Pace.stop();
        });
    }

});
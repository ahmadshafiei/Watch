var app = angular.module('watch');

app.controller('sellerListCtrl', function ($scope, sellerService, ngDialog, toaster) {

    $scope.result = {
        success: null,
        message: '',
    }

    $scope.pagination = {
        pageSize: 10,
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
            template: '/Statics/removeConfirmationPartial.html',
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



    function getAllStores() {
        $scope.pagination.loading = true;
        sellerService.getAllStores($scope.pagination.pageNumber, $scope.pagination.pageSize, $scope.pagination.searchExp).then(function (response) {
            $scope.stores = response.data.Result.Data;
            $scope.pagination.count = response.data.Result.Count;
            $scope.pagination.loading = true;
        }, function (response) {
            $scope.pagination.loading = true;
        });
    }

});
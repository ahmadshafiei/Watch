var app = angular.module('watch');

app.controller('sellerWatchesController', function ($scope, $cookies, ngDialog, sellerService, toaster) {

    $scope.brands = [];
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

    $scope.openWatchDetailDialog = function (watch) {
        $scope.watch = watch;

        $scope.watch.Gender = $scope.watch.Gender.toString();
        $scope.watch.Condition = $scope.watch.Condition.toString();
        if ($scope.watch.CounterMovement != null && $scope.watch.CounterMovement != undefined)
            $scope.watch.CounterMovement = $scope.watch.CounterMovement.toString();

        $scope.mode = 'edit';

        getAllBrands();

        ngDialog.open({
            template: '/Statics/SellerPanel/watchDetail.html',
            className: 'ngdialog-theme-default width60',
            scope: $scope,
            showClose: true,
        });
    }

    $scope.openInsertWatchDialog = function () {

        $scope.mode = 'insert';

        $scope.watch = { Condition: 0 };

        getAllBrands();

        ngDialog.open({
            template: '/Statics/SellerPanel/watchDetail.html',
            className: 'ngdialog-theme-default width60',
            scope: $scope,
            showClose: true,
        });
    }

    $scope.closeDialog = function () {
        ngDialog.closeAll();
    }

    $scope.insertWatch = function (watch) {

        watch.BrandId = watch.Brand.Id;

        delete watch.Brand;

        console.log(watch);

        sellerService.insertWatch(watch).then(function (response) {
            getStoreWatches();
            toaster.pop('success', 'ساعت مورد نظر با موفقیت افزوده شد');
            $scope.closeDialog();
        }, function (error) {
            toaster.pop('error', 'خطا در برقراری ارتباط با سرور');
            $scope.closeDialog();
        });
    }

    $scope.editWatch = function (watch) {

        sellerService.editWatch(watch).then(function (response) {
            getStoreWatches();
            toaster.pop('success', 'ساعت مورد نظر با موفقیت ویرایش شد');
            $scope.closeDialog();
        }, function (error) {
            toaster.pop('error', 'خطا در برقراری ارتباط با سرور');
            $scope.closeDialog();
        });


    }

    $scope.removeWatch = function (watchId) {

        sellerService.removeWatch(watchId).then(function (response) {
            getStoreWatches();
            toaster.pop('success', 'ساعت مورد نظر حذف شد');
            $scope.closeDialog();
        }, function (error) {
            toaster.pop('error', 'خطا در برقراری ارتباط با سرور');
            $scope.closeDialog();
        });

    }

    $scope.dzOptions = {
        url: '/api/Watch/UploadWatchImage',
        paramName: 'photo',
        dictDefaultMessage: 'عکس ساعت را انتخاب کنید',
        acceptedFiles: 'image/jpeg, images/jpg, image/png',
        addRemoveLinks: true,
        autoProcessQueue: false,
        maxFiles: 1,
        init: function () {
            myDropzone = this;

            $scope.addWatchImage = function () {
                myDropzone.processQueue();
            }
        }
    }

    $scope.dzCallbacks = {
        'addedfile': function (file) {
            $scope.newFile = file;
        },
        'success': function (response) {
            console.log(response);
            $scope.watch.MainImagePath = response.data;
            $scope.insertWatch($scope.watch);
        }
    };

    $scope.dzMethods = {};

    $scope.removeNewFile = function () {
        $scope.dzMethods.removeFile($scope.newFile);
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

    function getAllBrands() {
        sellerService.getAllBrands().then(function (response) {
            $scope.brands = response.data.Result.Data;
        });
    }

});
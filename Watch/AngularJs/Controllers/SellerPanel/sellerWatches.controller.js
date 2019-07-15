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
        $scope.watch = {};
        $scope.mode = 'insert';

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

        sellerService.insertWatch(watch).then(function (response) {
            getStoreWatches();
            toaster.pop('success', 'ساعت مورد نظر با موفقیت افزوده شد');
            $scope.closeDialog();
        }, function (error) {
            toaster.pop('error', 'خطا در برقراری ارتباط با سرور');
            $scope.closeDialog();
        });
    }

    $scope.editWatch = function () {

        sellerService.editWatch($scope.watch).then(function (response) {
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
        url: '/Api/Watch/UploadWatchImage',
        paramName: 'photo',
        dictDefaultMessage: 'عکس ساعت را انتخاب کنید',
        acceptedFiles: 'image/jpeg, images/jpg, image/png',
        addRemoveLinks: true,
        autoProcessQueue: false,
        maxFiles: 1,
        init: function () {
            mainImageDropzone = this;

            $scope.addWatchImage = function () {
                mainImageDropzone.processQueue();
                if (mainImageDropzone.files.length == 0)
                    $scope.addWatchImages();
            }

        }
    }

    $scope.dzCallbacks = {
        'addedfile': function (file) {
            $scope.newFile = file;
        },
        'success': function (response) {
            $scope.watch.MainImagePath = response.xhr.response.replace('"', '').replace('"', '');
        },
        'queuecomplete': function () {
            $scope.addWatchImages();
        }
    };

    $scope.dzMethods = {};

    $scope.removeNewFile = function () {
        $scope.dzMethods.removeFile($scope.newFile);
    }

    $scope.imagesDzOptions = {
        url: '/Api/Watch/UploadWatchImage',
        paramName: 'photo',
        dictDefaultMessage: 'عکس ساعت را انتخاب کنید',
        acceptedFiles: 'image/jpeg, images/jpg, image/png',
        addRemoveLinks: true,
        autoProcessQueue: false,
        maxFiles: 10,
        init: function () {
            imagesDropzone = this;

            $scope.addWatchImages = function () {
                imagesDropzone.processQueue();
                if (imagesDropzone.files.length == 0)
                    insertEditWatch();
            }
        }
    }

    $scope.imagesDzCallbacks = {
        'addedfile': function (file) {
            $scope.newFile = file;
        },
        'success': function (response) {
            if ($scope.watch.Images == undefined)
                $scope.watch.Images = [];
            $scope.watch.Images.push({ WatchId: $scope.watch.Id, path: response.xhr.response.replace('"', '').replace('"', '') });
        },
        'queuecomplete': function () {
            insertEditWatch();
        }
    };

    $scope.imagesDzMethods = {};

    $scope.imagesRemoveNewFile = function () {
        $scope.dzMethods.removeFile($scope.newFile);
    }

    $scope.removeWatchImage = function (imageId) {
        sellerService.removeWatchImage(imageId).then(function (response) {
            $scope.watch.Images = $scope.watch.Images.filter(i => i.Id != imageId);
            toaster.pop('success', 'عکس مورد نظر حذف شد');
        }, function (error) {
            toaster.pop('error', 'خطا در برقراری ارتباط با سرور');
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

    function getAllBrands() {
        sellerService.getAllBrands().then(function (response) {
            $scope.brands = response.data.Result.Data;
        });
    }

    function insertEditWatch() {
        console.log($scope.watch);
        if ($scope.mode == 'insert')
            $scope.insertWatch($scope.watch);
        else if ($scope.mode == 'edit')
            $scope.editWatch();
    }

});
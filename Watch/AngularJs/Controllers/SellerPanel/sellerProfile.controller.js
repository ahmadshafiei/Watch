var app = angular.module('watch');

app.controller('sellerProfileController', function ($scope, sellerService, toaster) {

    $scope.canEditProfile = {
        name: false,
        family: false,
        phoneNumber: false,
        nationalCode: false,
        storeName: false,
        storeTell: false,
        storePhoneNumber: false,
    }

    $scope.init = function () {
        getUserProfile();

    }

    $scope.editProfile = function () {
        sellerService.updateStoreProfile($scope.user).then(function (response) {
            toaster.pop('info', 'پروفایل با موفقیت ویرایش شد');
            $scope.user = response.data.Result.Data[0];
        });
    }

    $scope.removeStoreImage = function (imageId) {
        sellerService.removeStoreImage(imageId).then(function () {
            getUserProfile();
        });
    }


    $scope.dzOptions = {
        url: '/api/Store/UploadStoreImage',
        paramName: 'photo',
        dictDefaultMessage: 'عکس فروشگاه را انتخاب کنید',
        acceptedFiles: 'image/jpeg, images/jpg, image/png',
        addRemoveLinks: true,
        autoProcessQueue: false,
        maxFiles: 3,
        init: function () {
            myDropzone = this;

            $scope.addStoreImage = function () {
                if (!myDropzone.options.url.includes('storeId'))
                    myDropzone.options.url = myDropzone.options.url + '?storeId=' + $scope.user.Store.Id;
                
                myDropzone.processQueue();
            }
        }
    }

    $scope.dzCallbacks = {
        'addedfile': function (file) {
            $scope.newFile = file;
        },
        'success': function () {
            getUserProfile();
        }
    };

    $scope.dzMethods = {};

    $scope.removeNewFile = function () {
        $scope.dzMethods.removeFile($scope.newFile);
    }

    function getUserProfile() {

        sellerService.getUserProfile().then(function (response) {
            $scope.user = response.data.Result.Data[0];
        });

    }

});
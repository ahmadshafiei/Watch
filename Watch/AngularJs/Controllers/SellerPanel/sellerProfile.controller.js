var app = angular.module('watch');

app.controller('sellerProfile.controller', function ($scope, sellerService, toaster) {

    $scope.editProfile = {
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

        sellerService.editProfile($scope.user).then(function (response) {
            toaster.pop('info', 'پروفایل با موفقیت ویرایش شد');
            $scope.user = response.data.Result.Data[0];
        });

    }

    function getUserProfile() {

        sellerService.getUserProfile().then(function (response) {
            $scope.user = response.data.Result.Data[0];
        });

    }

});
var app = angular.module('watch');

app.controller('sellerProfile.controller', function ($scope, sellerService) {

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

        sellerService.editProfile().then(function (response) {

        });

    }

    function getUserProfile() {

        sellerService.getUserProfile().then(function (response) {
            $scope.user = response.data.Result.Data[0];
        });

    }

});
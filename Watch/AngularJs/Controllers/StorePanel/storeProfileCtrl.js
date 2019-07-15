var app = angular.module('watch');

app.controller('storeProfileCtrl', function ($scope, storeServices, ngDialog, toaster, NgMap) {

    $scope.init = function () {
        getStoreInfo();
    }

    function getStoreInfo() {
        Pace.start();
        storeServices.getStoreProfile().then(function (response) {
            $scope.storeInformation = response.data;
            Pace.stop();
        }, function (response) {
            Pace.stop();
        });
    }

});
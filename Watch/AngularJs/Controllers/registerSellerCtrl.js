var app = angular.module('watch');

app.controller('registerSellerCtrl', function ($scope, NgMap, sellerService) {

    //user Fetch
    var isFetching = false;
    $scope.userCount = 0;
    $scope.selectedUser = {};

    NgMap.getMap().then(function (map) {
        $scope.map = map;
        map.setZoom(13);
    });

    $scope.init = function () {
        $scope.getUsers('', 1);
    }

    $scope.seller = {
        storeName: '',
        tell: '',
        phoneNumber: '',
        userId: null,
        latitude: 35.6892,
        longitude: 51.3890,
    };

    $scope.addMarker = function (event) {
        var ll = event.latLng;
        $scope.seller.latitude = ll.lat();
        $scope.seller.longitude = ll.lng();
    }

    $scope.registerSeller = function () {
        console.log($scope.seller);
        sellerService.registerSeller($scope.seller).then(function (response) {
            $scope.result = {};
            if (response.Success) {
                $scope.result.type = 'success';
                $scope.result.message = 'فروشگاه با موفقیت ایجاد شد';
            }
            else {
                $scope.result.type = 'danger';
                $scope.result.message = response.Message;
            }
        }, function (response) {
            $scope.result = {};
            $scope.result.type = 'danger';
            $scope.result.message = response.data.ExceptionMessage;
        });
    }

    $scope.getUsers = function (searchExp, pageNumber) {
        if (isFetching) return;

        isFetching = true;

        sellerService.getUsers(searchExp, pageNumber)
            .then(function (result) {
                console.log(result);
                if (pageNumber == 1) {
                    $scope.userCount = result.data.Result.Count;
                    $scope.users = result.data.Result.Data;
                }
                else {
                    $scope.users = $scope.users.concat(result.data.Result.Data);
                }
                isFetching = false;
            },
            function (errorMessage) {
                console.log(errorMessage);
                isFetching = false;
            });
    }
});
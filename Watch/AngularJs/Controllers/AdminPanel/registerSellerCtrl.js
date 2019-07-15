var app = angular.module('watch');

app.controller('registerSellerCtrl', function ($scope, NgMap, $http, sellerService, toaster) {

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
        logoPath: '',
        user_Id: null,
        latitude: 35.6892,
        longitude: 51.3890,
    };

    $scope.addMarker = function (event) {
        var ll = event.latLng;
        $scope.seller.latitude = ll.lat();
        $scope.seller.longitude = ll.lng();
    }

    $scope.getUsers = function (searchExp, pageNumber) {
        if (isFetching) return;
        Pace.start();
        isFetching = true;

        sellerService.getUsers(searchExp, pageNumber)
            .then(function (result) {
                if (pageNumber == 1) {
                    $scope.userCount = result.data.Result.Count;
                    $scope.users = result.data.Result.Data;
                }
                else {
                    $scope.users = $scope.users.concat(result.data.Result.Data);
                }
                isFetching = false;
                Pace.stop();
            },
            function (errorMessage) {
                isFetching = false;
                Pace.stop();
            });
    }

    $scope.dzOptions = {
        url: '/api/Store/UploadStoreLogo',
        paramName: 'photo',
        acceptedFiles: 'image/jpeg, images/jpg, image/png',
        addRemoveLinks: true,
        autoProcessQueue: false,
        maxFiles: 1,
        init: function () {
            myDropzone = this;

            $scope.registerSeller = function () {
                Pace.start();
                $scope.disableAddBtn = true;
                var check = myDropzone.getQueuedFiles().length;
                myDropzone.processQueue();
                if (check == 0) {
                    add();
                }
            }

            function add() {
                $scope.seller.user_Id = $scope.selectedUser.Id;
                $http({ method: 'POST', url: '/api/Store/RegisterSeller', data: JSON.stringify($scope.seller), headers: { 'Content-type': 'application/json' } })
                    .then(function (response) {
                        $scope.disableAddBtn = false;
                        if (response.data.Success) {
                            toaster.pop('info', 'فروشگاه با موفقیت ایجاد شد');
                        }
                        else {

                            toaster.pop('error', 'خطا در برقراری ارتباط با سرور');
                        }
                        Pace.stop();
                    }, function () {
                        Pace.stop();
                        $scope.disableAddBtn = false;
                        toaster.pop('error', 'خطا در برقراری ارتباط با سرور');
                    });
            }

            this.on("queuecomplete", function (file) {
                add();
            });
        }

    }

    $scope.dzCallbacks = {
        'addedfile': function (file) {
            $scope.newFile = file;
        },
        'success': function (file, response) {
            $scope.seller.logoPath = response;
        },
    };

    $scope.dzMethods = {};

    $scope.removeNewFile = function () {
        $scope.dzMethods.removeFile($scope.newFile);
    }
});
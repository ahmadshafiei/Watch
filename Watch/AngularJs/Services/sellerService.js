var app = angular.module('watch');

app.service('sellerService', function ($http) {

    this.registerSeller = function (seller) {
        return $http({ method: 'POST', url: '/Api/Store/RegisterSeller', data: seller });
    }

    this.getUsers = function (searchExp, pageNumber) {
        return $http({ method: 'GET', url: '/Api/Store/GetAllUsers', params: { searchExp: searchExp, pageNumber: pageNumber } });
    }

});
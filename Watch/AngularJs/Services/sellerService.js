﻿var app = angular.module('watch');

app.service('sellerService', function ($http) {

    this.registerSeller = function (seller) {
        return $http({ method: 'POST', url: '/Api/Store/RegisterSeller', data: seller });
    }

    this.getUsers = function (searchExp, pageNumber) {
        return $http({ method: 'GET', url: '/Api/Store/GetAllUsers', params: { searchExp: searchExp, pageNumber: pageNumber } });
    }

    this.getAllStores = function (pageNumber, pageSize, searchExp) {
        return $http({ method: 'GET', url: '/Api/Store/GetAllStores', params: { pageNumber: pageNumber, pageSize: pageSize, searchExp: searchExp } });
    }

    this.removeStore = function (storeId) {
        console.log(storeId);
        return $http({ method: 'GET', url: '/Api/Store/RemoveStore', params: { storeId: storeId } });
    }

});
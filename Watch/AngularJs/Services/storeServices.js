var app = angular.module('watch');

app.service('storeService', function ($http) {

    this.getStoreProfile = function () {
        return $http({ method: 'GET', url: '/Api/Store/GetStoreProfile' });
    }

    this.getAllStoresWatches = function (pageNumber, pageSize, searchExp) {
        return $http({ method: 'GET', url: '/Api/Store/GetAllStoresWatches', params: { pageNumber: pageNumber, pageSize: pageSize, searchExp: searchExp } });
    }

    this.getAllStoresSoldItems = function (pageNumber, pageSize, searchExp) {
        return $http({ method: 'GET', url: '/Api/Store/GetAllStoresSoldItems', params: { pageNumber: pageNumber, pageSize: pageSize, searchExp: searchExp } });
    }

});
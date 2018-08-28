var app = angular.module('watch');

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
        return $http({ method: 'GET', url: '/Api/Store/RemoveStore', params: { storeId: storeId } });
    }

    this.getUserProfile = function () {
        return $http({ method: 'GET', url: '/Api/Store/GetUserProfile' });
    }

    this.updateStoreProfile = function (user) {
        return $http({ method: 'PUT', url: '/Api/Store/UpdateStoreProfile', data: user });
    }

    this.removeStoreImage = function (imageId) {
        return $http({ method: 'DELETE', url: '/Api/Store/RemoveStoreImage', params: { imageId: imageId } });
    }

    this.getStoreWatches = function (paginationData) {
        return $http({ method: 'GET', url: '/Api/Store/GetStoreWatches', params: paginationData });
    }

    this.getAllBrands = function () {
        return $http({ method: 'GET', url: '/Api/Watch/GetAllBrands' });
    }

    this.editWatch = function (watch) {
        return $http({ method: 'POST', url: '/Api/Watch/UpdateWatch', data: watch });
    }

    this.insertWatch = function (watch) {
        return $http({method:'POST' , url:'/Api/Watch/InsertWatch' , data:watch});
    }

    this.removeWatch = function (watchId) {
        return $http({ method: 'GET', url: '/Api/Watch/DeleteWatch?id=' + watchId });
    }

});
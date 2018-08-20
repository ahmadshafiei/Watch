var app = angular.module('watch');

app.config(['$stateProvider', '$urlRouterProvider', '$httpProvider', 'dropzoneOpsProvider', function ($stateProvider, $urlRouterProvider, $httpProvider, dropzoneOpsProvider) {

    $httpProvider.interceptors.push(['$location', '$injector', '$q', '$cookies', function ($location, $injector, $q, $cookies) {
        return {
            'responseError': function (rejection) {

                if (rejection.status === 401) {
                    $injector.get('$state').go('forbidden');
                    return $q.reject(rejection);
                }
            }
        };
    }]);

    $stateProvider
        .state('forbidden', {
            url: '/NotFound',
            templateUrl: '/Statics/401.html',
        })
        .state('notFound', {
            url: '/NotFound',
            templateUrl: '/Statics/404.html',
        })
        .state('login', {
            url: '/Home/Login',
            controller: 'loginCtrl',
        })
        .state('adminPanel', {
            url: '/Panel',
            templateUrl: '/Statics/index.html',
        })
        .state('sellerPanel', {
            url: '/Panel',
            templateUrl: '/Statics/index.html',
        })
        .state('adminPanel.registerSeller', {
            url: '/RegisterSeller',
            templateUrl: '/Statics/AdminPanel/registerSeller.html',
            controller: 'registerSellerCtrl'
        })
        .state('adminPanel.sellerList', {
            url: '/SellerList',
            templateUrl: '/Statics/AdminPanel/sellerList.html',
            controller: 'sellerListCtrl'
        })
        .state('sellerPanel.profile', {
            url: '/Profile',
            templateUrl: '/Statics/SellerPanel/profile.html',
            controller: 'sellerProfile.controller'
        })
        .state('sellerPanel.watches', {
            url: '/Watches',
            templateUrl: '/Statics/SellerPanel/watches.html',
            controller: 'sellerWatches.controller'
        })
        .state('sellerPanel.salesHistory', {
            url: '/SalesHistory',
            templateUrl: '/Statics/SellerPanel/salesHistory.html',
            controller: 'sellerSalesHistory.controller'
        });

}]).run(['authenticationService', '$state', '$rootScope', function (authenticationService, $state, $rootScope) {

    authenticationService.getCurrentRoles().then(function (response) {
        if (response.data.includes('Admin')) {
            $rootScope.role = 'Admin';
            $state.go('adminPanel.registerSeller');
        }
        else if (response.data.includes('Seller')) {
            $rootScope.role = 'Seller';
            $state.go('sellerPanel.profile');
        }
    });

}]);;
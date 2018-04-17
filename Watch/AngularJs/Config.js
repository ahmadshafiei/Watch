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

    $urlRouterProvider.otherwise('/AdminPanel/RegisterSeller');

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
            url: '/AdminPanel',
            templateUrl: '/Statics/index.html',
        })
        .state('adminPanel.registerSeller', {
            url: '/RegisterSeller',
            templateUrl: '/Statics/registerSeller.html',
            controller: 'registerSellerCtrl'
        })
        .state('adminPanel.sellerList', {
            url: '/SellerList',
            templateUrl: '/Statics/sellerList.html',
            controller: 'sellerListCtrl'
        });

}]);
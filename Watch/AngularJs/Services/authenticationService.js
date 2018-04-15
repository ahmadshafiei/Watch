var app = angular.module('watch');

app.service('authenticationService', function ($http) {

    this.login = function (user) {
        return $http({
            method: 'POST',
            url: '/Token',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            data: { grant_type: 'password', username: user.userName, password: user.password }
        });
    };

});
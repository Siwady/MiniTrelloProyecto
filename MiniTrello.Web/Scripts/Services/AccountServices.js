'use strict';

angular.module('app.services', []).factory('AccountServices', ['$http', function ($http) {

    var baseRemoteUrl = "http://minitrelloapis.apphb.com";
    var baseLocalUrl = "http://localhost:1416";
    var baseUrl = baseRemoteUrl;

    var account = {};


    account.login = function (data) {
        return $http.post(baseUrl + '/login', data);
    };

    account.register = function (data) {
        return $http.post(baseUrl + '/register', data);
    };

    return account;

}]);
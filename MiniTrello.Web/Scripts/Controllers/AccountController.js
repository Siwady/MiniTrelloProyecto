﻿'use strict';

// Google Analytics Collection APIs Reference:
// https://developers.google.com/analytics/devguides/collection/analyticsjs/

angular.module('app.controllers')



    // Path: /login
    .controller('AccountController', ['$scope', '$location', '$window', 'AccountServices', function ($scope, $location, $window, AccountServices) {



        $scope.$root.title = 'AngularJS SPA | Sign In';

        $scope.Email = "";

        $scope.Password = "";

        // TODO: Authorize a user
        $scope.login = function () {
            var model = { Email: $scope.Email, Password: $scope.Password };
            AccountServices.login(model);
            $location.path('/login');
            return false;
        };

        $scope.register = function () {
            var model = {
                FirstName: $scope.FirstName, LastName: $scope.LastName,
                Email: $scope.Email, Password: $scope.Password, ConfirmPassword: $scope.ConfirmPassword
            };
            AccountServices.register(model);
            return false;
        };

        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });
    }]);
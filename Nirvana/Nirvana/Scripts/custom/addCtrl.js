﻿angular.module('my_nirvana')
    .controller('addCtrl', function ($scope, $http, $location) {
        
        $scope.new_act = {};
        $scope.postActs = function () {
            $http({
                method: 'POST',
                url: '/api/Acts',
                data: $scope.new_act
            }).then({
                function(response) {
                    debugger;
                }, function(response) {
                    debugger;
                }
            })
        }
    })
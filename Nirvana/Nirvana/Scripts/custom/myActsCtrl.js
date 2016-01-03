angular.module('my_nirvana')
    .controller('myActsCtrl', function ($scope, $http, $location) {
        $http.get("api/Acts/GetCurrentUserActs").then(
            function (response) {
            $scope.myActs = response.data;
            },
            function (response) {
            return console.log("failure @ myActs");
            }
        );
    });
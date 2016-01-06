angular.module('my_nirvana')
    .controller('myActsCtrl', function ($scope, $http, $location) {
       $http.get("api/Acts/CurrentUserRank").then(
            function (response) {
               console.log("got the user rank!");
               $scope.UserRank = response.data;
               },
            function (response) {
                return console.log("failure @ user get rank");
            }
         );

        $http.get("api/Acts/GetCurrentUserActs").then(
            function (response) {
                $scope.myActs = response.data;
                if (response.data.length === 0)
                {
                    $scope.sorryMsg = "You don't have any acts. Please log an act to gain points!";
                }
            },
            function (response) {
            return console.log("failure @ myActs");
            }
        );

    });
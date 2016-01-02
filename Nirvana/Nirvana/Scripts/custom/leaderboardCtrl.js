angular.module('my_nirvana')
    .controller('leaderboardCtrl', function ($scope, $http, $location) {
        debugger
        $http.get("api/Acts/Leaderboard").then(
            function (response) {
                $scope.allRanks = response.data;
                console.log("SUCCESS on Leaderboard");
            }, 
            function (response) {
                console.log("here's the error response", response);
            }
          )

        $scope.sample = "what's up leaderboard!!";

    });
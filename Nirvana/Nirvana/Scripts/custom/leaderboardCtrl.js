angular.module('my_nirvana')
    .controller('leaderboardCtrl', function ($scope, $http, $location) {
        $http.get("api/Acts/Leaderboard").then(
            function (response) {
                var asJSON = JSON.parse(response.data);
                $scope.allRanks = asJSON;
                console.log(response.data, "here's the leaderboard data");
                console.log("SUCCESS on Leaderboard");
            }, 
            function (response) {
                console.log("here's the error response", response);
            }
          )

        $scope.sample = "what's up leaderboard!!";

    });
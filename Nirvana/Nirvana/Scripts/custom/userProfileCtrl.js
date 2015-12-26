angular.module('my_nirvana')
    .controller('userProfileCtrl', function ($scope, $http, $location) {
        $http.get('api/GetAllActs').then(
            function(response) {
                $scope.things = response.data;
                console.log('SUCCESS!');
            }, function(response) {
                return console.log('Failure - @ getallacts')
            }
        );

        $scope.postComment = function (act, comment) {
            debugger
            var url = "api/Acts/"+act.randomActId;
            $http.post(url, comment).then(
              function (response) { console.log("SUCCESS - comments using FROMURI"); },
              function (response) { console.log("ERRORRRRRR - comments using FROM URI"); }
            )
        };
    });
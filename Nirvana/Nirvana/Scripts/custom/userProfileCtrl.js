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
            var newComment = {
                userComment: comment, actId: act.RandomActId
            }

            var toSend = JSON.stringify(newComment);

            var url = "api/Acts/" + act;
           
            $http.post(url, toSend).then(
              function (response) { console.log("SUCCESS - comments using FROMURI"); },
              function (response) { console.log("ERRORRRRRR - comments using FROM URI"); }
            )
        };
    });
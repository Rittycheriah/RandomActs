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
            var newComment = {
                userComment: comment, actId: act
            }

            var toSend = JSON.stringify(newComment);
            
            var url = "/api/Acts/" + act;
           
            $http.post(url, toSend).then(
              function (response) { console.log("SUCCESS - comments using FROMURI"), isCommenting = false },
              function (response) { console.log("ERRORRRRRR - comments using FROM URI"); }
            )
        };

        $scope.showActComments = function (act_id) {
            debugger
            $scope.showComments = true;
            $http({
                url: "api/Acts/GetComms/" + act_id, 
                method: "GET", 
                params: {randomActId: act_id}
                }).then(
                function (response) {
                    $scope.comments = response.data;
                    console.log('SUCCESS! - comments GET');
                }, function (response) {
                    return console.log('Failure - @ getallcomments')
                }
             );
        };
    });
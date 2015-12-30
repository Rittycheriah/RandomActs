angular.module('my_nirvana')
    .controller('userProfileCtrl', function ($scope, $http, $location) {
        $http.get("api/Acts/GetCurrentUser").then(
            function (response) {
                console.log("got the user!");
                $scope.theUser = response;
            }, 
            function (response){
                return console.log("failure @ user get");
            }
        );

        $http.get("api/Acts/CurrentUserRank").then(
            function (response) {
                                console.log("got the user rank!");
                                $scope.theUser = response;
                            },
            function (response) {
                return console.log("failure @ user get rank");
            }
            )

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
              function (response) { console.log("SUCCESS - comments using FROMURI"), isCommenting = false, $scope.showActComments(act) },
              function (response) { console.log("ERRORRRRRR - comments using FROM URI"); }
            )
        };

        $scope.current_comment = false;
        $scope.editingComment = false;

        $scope.showActComments = function (act_id) {
            $scope.current_comment = true;
            $scope.showComments = true;
            $http({
                url: "api/Acts/GetComms/" + act_id, 
                method: "GET", 
                params: {randomActId: act_id}
                }).then(
                function (response) {
                    $scope.comments = response.data;
                    var actID = $scope.comments[0].actId;
                    var found_act = _.find($scope.things, function (act) {
                        return act.randomActId === actID;
                    });
                    debugger
                    found_act.comments = $scope.comments;
                }, function (response) {
                    console.log("ERRORR - GET comments");
                }
             );
        };

        $scope.deleteComment = function (comment_id) {
            var url = "/api/Acts/DeleteComm/" + comment_id;
            $http({
                url: url,
                method: "DELETE",
            }).then(
                function () {
                    console.log("DELETED SUCCESS");
                    var the_deleted = angular.element(document.querySelector('comments.commentId'));
                    the_deleted.empty();
                },
                function () {
                    console.log("DELETED FAIL");
                });

        }

        $scope.editComment = function (comment_id, comment){
            var editComment = {
                userComment: comment
            }

            var toSend = JSON.stringify(editComment);

            var url = "/api/Acts/EditComm/" + comment_id;
            $http.put(url, toSend).then(
                function () {
                    console.log("edit success!");
                    $scope.editingComment = false;
                }, 
                function () {
                    console.log("edit not successful");
                }
            )
        }

    });
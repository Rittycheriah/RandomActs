angular.module('my_nirvana')
    .controller('userProfileCtrl', function ($scope, $http, $location, $route) {
        $scope.isCommenting = false;
        $scope.current_comment = false;

        $http.get("api/Acts/GetCurrentUser").then(
            function (response) {
                console.log("got the user!");
                $scope.UserName = response;
            }, 
            function (response){
                return console.log("failure @ user get");
            }
        );

        $http.get("api/Acts/CurrentUserRank").then(
            function (response) {
               console.log("got the user rank!");
               $scope.UserRank = response;
               },
            function (response) {
                return console.log("failure @ user get rank");
            }
           );

        $http.get("api/Acts/TotalUserPts").then(
            function (response) {
                console.log("got the user total pts");
                $scope.UserTotal = response; 
            },
            function () {
                return console.log("failure @ user get total");
            }
         )

        $http.get('api/GetAllActs').then(
            function(response) {
                $scope.things = response.data;
                console.log(response.data, "here's all the acts");
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
              function (response) { console.log("SUCCESS - comments using FROMURI"); $scope.closeIt = true; $scope.showActComments(act) },
              function (response) { console.log("ERRORRRRRR - comments using FROM URI"); }
            )
        };

        $scope.showActComments = function (act_id) {
            $scope.current_comment = true;
            $scope.showComments = true;
            debugger
            $http({
                url: "api/Acts/GetComms/" + act_id, 
                method: "GET", 
                params: {randomActId: act_id}
            }).then(
                function (response) {
                    $scope.comments = response.data;
                    if(response.data.length == 0){
                        $scope.comment = ["There are no comments for this act"];
                    } else {
                        var actID = $scope.comments[0].actId;
                        found_act = _.find($scope.things, function (act) {
                        return act.randomActId === actID;
                        });
                        found_act.comments = $scope.comments;
                    };
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
                    reloadRoute();
                },
                function () {
                    console.log("DELETED FAIL");
                });

        }

        $scope.editComment = function (comment_id, comment) {
            $scope.waiting = true;
            var editComment = {
                userComment: comment
            }

            var toSend = JSON.stringify(editComment);

            var url = "/api/Acts/EditComm/" + comment_id;
            $http.put(url, toSend).then(
                function () {
                    console.log("edit success!");
                    $scope.editingComment = false;
                    $scope.waiting = false;
                }, 
                function () {
                    console.log("edit not successful");
                }
            )
        }

        $scope.postLike = function (act_id) {
            var url = "/api/Acts/PostLike/" + act_id;

            $http.post(url).then(
              function (response) {
                  console.log("SUCCESS - LIKES")
                  reloadRoute();
              },
              function (response) {
                  console.log("ERRORRRRRR - LIKES");
                  $scope.likeError = "Sorry, you've already liked that!"
              }
            )
        }

        $scope.changeEdit = function (comm_id){
            return $scope.editingComment = comm_id;
        }

        var reloadRoute = function () {
            $route.reload();
        }
    });
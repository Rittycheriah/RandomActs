angular.module('my_nirvana')
    .controller('userProfileCtrl', function ($scope, $http, $location) {

        $scope.sample = "at user profile";
        debugger
        $http.get('api/GetAllActs').then(
            function(response) {
                debugger
                $scope.things = response.data;
                console.log('SUCCESS!');
            }, function(response) {
                return console.log('Failure - @ getallacts')
            }
        );
    });
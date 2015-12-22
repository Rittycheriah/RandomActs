angular.module('my_nirvana')
    .controller('userProfileCtrl', function ($scope, $http, $location) {

        var get_all_acts = function () {
            $http({
                method: 'GET',
                url: '/api/Acts'
            }).then({
                function(response) {
                    $scope.all = data;
                    debugger
                    console.log('SUCCESS!');
                }, function(response) {
                    debugger
                    console.log('Failure - @ getallacts')
                }
            })
        };

        get_all_acts();
    });
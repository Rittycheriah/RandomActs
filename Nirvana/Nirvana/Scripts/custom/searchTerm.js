angular.module('my_nirvana')
    .controller('searchTerm', function ($scope, $http, $location) {
        $scope.searchTerm = function (Term) {
            debugger
            $http({
                url: "api/Acts/Search/",
                method: "GET",
                params: { term: Term }
            }).then(
                function (response) {
                    $scope.found = response.data;
                    console.log("search Complete!")
                }, function (response) {
                    console.log("ERRORR - GET search");
                }
             );
        };
    });
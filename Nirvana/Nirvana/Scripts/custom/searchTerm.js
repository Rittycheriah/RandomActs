angular.module('my_nirvana')
    .controller('searchTerm', function ($scope, $http, $location) {
        $http.get("api/Acts/Search/").then(
                function (response) {
                    var array4Filter = [];
                    for (var i = 0; i < response.data.length; i++)
                    {
                        array4Filter.push(response.data[i].randomActTitle);
                    }
                    $scope.found_acts = array4Filter;
                    console.log("search Complete!");
                    console.log(array4Filter);
                }, function (response) {
                    console.log("ERRORR - GET search");
                });
    });
angular.module('my_nirvana')
   .filter('array', function() {
    return function(items) {
        var filtered = [];
        angular.forEach(items, function(item) {
            filtered.push(item);
        });
        return filtered;
      };
    })
    .filter('term', function () {
        return function (search) {
            return act.filter(function (act) {
                var arrayOWords = act.split('');
                for (var i in arrayOWords) {
                    if (arrayOWords.indexOf(search) != -1) {
                        return true;
                    }
                }
                return false;
            });
        };
    })
    .controller('searchTerm', function ($scope, $http, $location) {
            debugger
            $http.get("api/Acts/Search/").then(
                function (response) {
                    $scope.found_acts = response.data;
                    console.log("search Complete!")
                }, function (response) {
                    console.log("ERRORR - GET search");
                });
    });
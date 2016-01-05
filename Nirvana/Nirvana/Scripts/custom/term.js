angular.module('my_nirvana').filter('term', function () {
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
});
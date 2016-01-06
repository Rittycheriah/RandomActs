angular.module('nirvana_filters', []).filter('term', function () {
    return function (leTerm) {
        if (leTerm.indexOf(leTerm) != -1) {
            return true;
        }
        return false;
  };
}).filter('array', function() {
    return function(items) {
        var filtered = [];
        angular.forEach(items, function(item) {
            filtered.push(item);
        });
        return filtered;
      };
  });
    
//return act.filter(function (act) {
//    var arrayOWords = act.split('');
//    for (var i in arrayOWords) {
//        if (arrayOWords.indexOf(search) != -1) {
//            return true;
//        }
//    }
//    return false;
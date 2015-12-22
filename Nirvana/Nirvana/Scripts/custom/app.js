'use strict';

var nirvana_mod = angular.module('my_nirvana', ['ngRoute', 'ui.bootstrap']);

nirvana_mod.config(function ($routeProvider, $locationProvider) {
    $locationProvider.html5Mode({
        enabled: true,
        requiredBase: false
    });
    $routeProvider.
      when('/', {
          templateUrl: '/templates/splash.html',
          controller: 'splashCtrl'
      }).
      when('/userProfile', {
          templateUrl: '/templates/user_profile.html',
          controller: 'userProfileCtrl'
      }).
      when('/addAct', {
          templateUrl: '/templates/add_acts.html',
          controller: 'addCtrl'
      }).
      otherwise({
          redirectTo: '/'
      });
});
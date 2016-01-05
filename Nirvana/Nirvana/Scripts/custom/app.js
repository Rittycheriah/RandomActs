'use strict';

var nirvana_mod = angular.module('my_nirvana', ['ngRoute', 'ui.bootstrap']);

nirvana_mod.config(function ($routeProvider, $locationProvider) {
    $routeProvider.
      when('/', {
          templateUrl: '/templates/splash.html',
          controller: 'splashCtrl'
      }).
      when('/userProfile', {
          templateUrl: '/templates/user_profile.html',
          controller: 'userProfileCtrl'
      }).
      when('/myActs', {
          templateUrl: '/templates/myActs.html',
          controller: 'myActsCtrl'
      }).
      when('/addAct', {
          templateUrl: '/templates/add_acts.html',
          controller: 'addCtrl'
      }).
      when('/leaderboard', {
          templateUrl: '/templates/leaderboard.html', 
          controller: 'leaderboardCtrl'
      }).
      when('/search', {
          templateUrl: '/templates/Search.html',
          controller: 'searchTerm'
      }).
      otherwise({
          redirectTo: '/'
      });
});
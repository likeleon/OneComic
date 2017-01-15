var accountRegisterModule = angular.module('accountRegister', ['common']);

accountRegisterModule.controller('AccountRegisterViewModel', function ($scope, $http, $location, $window, viewModelHelper) {

  $scope.viewModelHelper = viewModelHelper;
  
  $scope.accountModelStep1 = new OneComic.AccountRegisterModelStep1();
  $scope.accountModelStep2 = new OneComic.AccountRegisterModelStep2();

  $scope.previous = function () {
    $window.history.back();
  }
});
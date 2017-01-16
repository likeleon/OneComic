var accountRegisterModule = angular.module('accountRegister', ['common'])
  .config(function ($routeProvider, $locationProvider) {
    $routeProvider.when(OneComic.rootPath + 'account/register/step1', { templateUrl: OneComic.rootPath + 'Templates/RegisterStep1.html', controller: 'AccountRegisterStep1ViewModel' });
    $routeProvider.when(OneComic.rootPath + 'account/register/step2', { templateUrl: OneComic.rootPath + 'Templates/RegisterStep2.html', controller: 'AccountRegisterStep2ViewModel' });
    $routeProvider.when(OneComic.rootPath + 'account/register/confirm', { templateUrl: OneComic.rootPath + 'Templates/RegisterConfirm.html', controller: 'AccountRegisterConfirmViewModel' });
    $routeProvider.otherwise({ redirectTo: OneComic.rootPath + 'account/register/step1' });
    $locationProvider.html5Mode(true);
  });

accountRegisterModule.controller('AccountRegisterViewModel', function ($scope, $http, $location, $window, viewModelHelper) {

  $scope.viewModelHelper = viewModelHelper;
  
  $scope.accountModelStep1 = new OneComic.AccountRegisterModelStep1();
  $scope.accountModelStep2 = new OneComic.AccountRegisterModelStep2();

  $scope.previous = function () {
    $window.history.back();
  }
});

accountRegisterModule.controller("AccountRegisterStep1ViewModel", function ($scope, $http, $location, $window, viewModelHelper, validator) {

});

accountRegisterModule.controller("AccountRegisterStep2ViewModel", function ($scope, $http, $location, $window, viewModelHelper, validator) {

});

accountRegisterModule.controller("AccountRegisterConfirmViewModel", function ($scope, $http, $location, $window, viewModelHelper, validator) {

});
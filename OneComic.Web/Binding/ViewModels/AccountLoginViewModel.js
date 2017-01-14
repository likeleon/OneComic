appMainModule.controller("AccountLoginViewModel", function ($scope, $http, viewModelHelper, validator) {

  $scope.viewModelHelper = viewModelHelper;
  $scope.accountModel = new OneComic.AccountLoginModel();
  $scope.returnUrl = '';

  $scope.login = function () {
    alert('hello viewer');
  }
});
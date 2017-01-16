var accountRegisterModule = angular.module('accountRegister', ['common'])
  .config(function ($routeProvider, $locationProvider) {
    $routeProvider.when(OneComic.rootPath + 'account/register/step1', { templateUrl: OneComic.rootPath + 'Templates/RegisterStep1.html', controller: 'AccountRegisterStep1ViewModel' });
    $routeProvider.when(OneComic.rootPath + 'account/register/step2', { templateUrl: OneComic.rootPath + 'Templates/RegisterStep2.html', controller: 'AccountRegisterStep2ViewModel' });
    $routeProvider.when(OneComic.rootPath + 'account/register/confirm', { templateUrl: OneComic.rootPath + 'Templates/RegisterConfirm.html', controller: 'AccountRegisterConfirmViewModel' });
    $routeProvider.otherwise({ redirectTo: OneComic.rootPath + 'account/register/step1' });
    $locationProvider.html5Mode({
      enabled: true,
      requireBase: false
    });
  });

accountRegisterModule.controller("AccountRegisterViewModel", function ($scope, $http, $location, $window, viewModelHelper) {

  $scope.viewModelHelper = viewModelHelper;
  
  $scope.accountModelStep1 = new OneComic.AccountRegisterModelStep1();
  $scope.accountModelStep2 = new OneComic.AccountRegisterModelStep2();

  $scope.previous = function () {
    $window.history.back();
  }
});

accountRegisterModule.controller("AccountRegisterStep1ViewModel", function ($scope, $http, $location, $window, viewModelHelper, validator) {

  viewModelHelper.modelIsValid = true;
  viewModelHelper.modelErrors = [];

  var accountModelStep1Rules = [];

  var setupRules = function () {
    accountModelStep1Rules.push(new validator.PropertyRule("FirstName", {
      required: { message: "First name is required" }
    }));
    accountModelStep1Rules.push(new validator.PropertyRule("LastName", {
      required: { message: "Last name is required" }
    }));
  }

  $scope.step2 = function () {
    validator.ValidateModel($scope.accountModelStep1, accountModelStep1Rules);
    viewModelHelper.modelIsValid = $scope.accountModelStep1.isValid;
    viewModelHelper.modelErrors = $scope.accountModelStep1.errors;
    if (viewModelHelper.modelIsValid) {
      viewModelHelper.apiPost('api/account/register/validate1', $scope.accountModelStep1,
        function (result) {
          $scope.accountModelStep1.Initialized = true;
          $location.path(OneComic.rootPath + 'account/register/step2');
        });
    }
  }

  setupRules();
});

accountRegisterModule.controller("AccountRegisterStep2ViewModel", function ($scope, $http, $location, $window, viewModelHelper, validator) {

  if (!$scope.accountModelStep1.Initialized) {
    $location.path(OneComic.rootPath + 'account/register/step1');
  }

  viewModelHelper.modelIsValid = true;
  viewModelHelper.modelErrors = [];

  var accountModelStep2Rules = [];

  var setupRules = function () {
    accountModelStep2Rules.push(new validator.PropertyRule("LoginEmail", {
      required: { message: "Login Email is required" },
    }));
    accountModelStep2Rules.push(new validator.PropertyRule("Password", {
      required: { message: "Password name is required" },
      minLength: { message: "Password must be at least 6 characters", params: 6 }
    }));
    accountModelStep2Rules.push(new validator.PropertyRule("PasswordConfirm", {
      required: { message: "Password confirmation is required" },
      custom: {
        validator: OneComic.mustEqual,
        message: "Password do not match",
        params: function () { return $scope.accountModelStep2.Password; }
      }
    }));
  }

  $scope.confirm = function () {
    validator.ValidateModel($scope.accountModelStep2, accountModelStep2Rules);
    viewModelHelper.modelIsValid = $scope.accountModelStep2.isValid;
    viewModelHelper.modelErrors = $scope.accountModelStep2.errors;
    if (viewModelHelper.modelIsValid) {
      viewModelHelper.apiPost('api/account/register/validate2', $scope.accountModelStep2,
        function (result) {
          $scope.accountModelStep2.Initialized = true;
          $location.path(OneComic.rootPath + 'account/register/confirm');
        });
    }
  }

  setupRules();
});

accountRegisterModule.controller("AccountRegisterConfirmViewModel", function ($scope, $http, $location, $window, viewModelHelper, validator) {

  if (!$scope.accountModelStep2.Initialized) {
    $location.path(OneComic.rootPath + 'account/register/step1');
  }

  $scope.createAccount = function () {

    var accountModel;

    accountModel = $.extend(accountModel, $scope.accountModelStep1);
    accountModel = $.extend(accountModel, $scope.accountModelStep2);

    viewModelHelper.apiPost('api/account/register', accountModel,
      function (result) {
        $window.location.href = OneComic.rootPath;
      });
  }
});
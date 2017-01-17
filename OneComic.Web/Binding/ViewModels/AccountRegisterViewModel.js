appMainModule.controller("AccountRegisterViewModel", function ($scope, $http, $location, $window, viewModelHelper, validator) {

  $scope.viewModelHelper = viewModelHelper;
  $scope.accountModel = new OneComic.AccountRegisterModel();
  $scope.returnUrl = '';

  var accountModelRules = [];

  var setupRules = function () {
    accountModelRules.push(new validator.PropertyRule("LoginEmail", {
      required: { message: "Login Email is required" },
    }));
    accountModelRules.push(new validator.PropertyRule("Password", {
      required: { message: "Password name is required" },
      minLength: { message: "Password must be at least 6 characters", params: 6 }
    }));
    accountModelRules.push(new validator.PropertyRule("PasswordConfirm", {
      required: { message: "Password confirmation is required" },
      custom: {
        validator: OneComic.mustEqual,
        message: "Password do not match",
        params: function () { return $scope.accountModel.Password; }
      }
    }));
  }

  $scope.createAccount = function () {

    validator.ValidateModel($scope.accountModel, accountModelRules);
    viewModelHelper.modelIsValid = $scope.accountModel.isValid;
    viewModelHelper.modelErrors = $scope.accountModel.errors;
    if (!viewModelHelper.modelIsValid) {
      return;
    }

    viewModelHelper.apiPost('api/account/register', $scope.accountModel,
      function (result) {
        $window.location.href = OneComic.rootPath;
      });
  }

  setupRules();
});

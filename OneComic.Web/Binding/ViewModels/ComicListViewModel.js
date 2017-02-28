appMainModule.controller("ComicListViewModel", function ($scope, viewModelHelper) {
  $scope.comics = [];
  $scope.init = false;

  $scope.browseCars = function () {
    viewModelHelper.apiGet("comics", null,
      function (result) {
        $scope.comics = result.data;
        $scope.init = true;
      });
  }

  $scope.browseCars();
});
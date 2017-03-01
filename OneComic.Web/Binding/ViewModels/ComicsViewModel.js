appMainModule.controller("ComicsViewModel", function ($scope, viewModelHelper) {
  $scope.init = false;
  $scope.comics = [];

  $scope.browseComics = function (page) {
    var requestUri = "comics?page=" + page + "&pageSize=" + 2;
    viewModelHelper.apiGet(requestUri, null,
      function (result) {
        var pagination = angular.fromJson(result.headers('X-Pagination'));

        $scope.comics = result.data;
        $scope.currentPage = pagination.currentPage;
        $scope.itemsPerPage = pagination.pageSize;
        $scope.totalItems = pagination.totalCount;

        $scope.init = true;
      });
  };

  $scope.pageChanged = function () {
    $scope.browseComics($scope.currentPage);
  };

  $scope.browseComics(1);
});
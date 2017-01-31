appMainModule.controller("ComicListViewModel", function ($scope) {
  $scope.comics = [];
  for (var i = 0; i < 10; ++i) {
    var comic = new Object();
    comic.CoverImage = "https://media.manaa.space/comics/3627/thumbnail.jpeg";
    comic.Title = "사카모토입니다만?"
    comic.Description = "사노 나미, 학원/드라마/개그";
    $scope.comics.push(comic);
  }
});
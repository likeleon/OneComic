using Core.Common.ServiceModel;
using OneComic.Client.Contracts;
using OneComic.Client.Entities;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace OneComic.Client.Proxies
{
    [Export(typeof(ILibraryService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class LibraryClient : UserClientBase<ILibraryService>, ILibraryService
    {
        public void DeleteComic(int comicId) => Channel.DeleteComic(comicId);

        public Task DeleteComicAsync(int comicId) => Channel.DeleteComicAsync(comicId);

        public Comic[] GetAllComics() => Channel.GetAllComics();

        public Task<Comic[]> GetAllComicsAsync() => Channel.GetAllComicsAsync();

        public Comic GetComic(int comicId) => Channel.GetComic(comicId);

        public Task<Comic> GetComicAsync(int comicId) => Channel.GetComicAsync(comicId);

        public Comic UpdateComic(Comic comic) => Channel.UpdateComic(comic);

        public Task<Comic> UpdateComicAsync(Comic comic) => UpdateComicAsync(comic);
    }
}

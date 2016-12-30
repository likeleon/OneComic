using OneComic.Business.Entities;
using System.ServiceModel;

namespace OneComic.Business.Contracts
{
    [ServiceContract]
    public interface ILibraryService
    {
        [OperationContract]
        Comic GetComic(int comicId);

        [OperationContract]
        Comic[] GetAllComics();
    }
}

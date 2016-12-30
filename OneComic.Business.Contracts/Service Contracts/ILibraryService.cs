using Core.Common.Exceptions;
using OneComic.Business.Entities;
using System.ServiceModel;

namespace OneComic.Business.Contracts
{
    [ServiceContract]
    public interface ILibraryService
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Comic GetComic(int comicId);

        [OperationContract]
        Comic[] GetAllComics();
    }
}

using Core.Common.Contracts;
using Core.Common.Exceptions;
using OneComic.Client.Entities;
using System.ServiceModel;
using System.Threading.Tasks;

namespace OneComic.Client.Contracts
{
    [ServiceContract]
    public interface ILibraryService : IServiceContract
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Comic GetComic(int comicId);

        [OperationContract]
        Comic[] GetAllComics();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Comic UpdateComic(Comic comic);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void DeleteComic(int comicId);

        [OperationContract]
        Task<Comic> GetComicAsync(int comicId);

        [OperationContract]
        Task<Comic[]> GetAllComicsAsync();

        [OperationContract]
        Task<Comic> UpdateComicAsync(Comic comic);

        [OperationContract]
        Task DeleteComicAsync(int comicId);
    }
}

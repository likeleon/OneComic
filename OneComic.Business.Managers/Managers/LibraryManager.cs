using Core.Common.Contracts;
using Core.Common.Exceptions;
using OneComic.Business.Contracts;
using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;

namespace OneComic.Business.Managers.Managers
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public sealed class LibraryManager : Manager, ILibraryService
    {
#pragma warning disable 0649
        [Import]
        private IDataRepositoryFactory _dataRepositoryFactory;
#pragma warning restore 0649

        public Comic[] GetAllComics()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var comicRepository = _dataRepositoryFactory.GetDataRepository<IComicRepository>();
                return comicRepository.Get().ToArray();
            });
        }

        public Comic GetComic(int comicId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var comicRepository = _dataRepositoryFactory.GetDataRepository<IComicRepository>();
                var comic = comicRepository.Get(comicId);
                if (comic == null)
                {
                    var ex = new NotFoundException($"Comic with ID of {comicId} is not in database");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                return comic;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Comic UpdateComic(Comic comic)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var comicRepository = _dataRepositoryFactory.GetDataRepository<IComicRepository>();
                if (comic.ComicId == 0)
                    return comicRepository.Add(comic);
                else
                    return comicRepository.Update(comic);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void DeleteComic(int comicId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var comicRepository = _dataRepositoryFactory.GetDataRepository<IComicRepository>();
                comicRepository.Remove(comicId);
            });
        }
    }
}

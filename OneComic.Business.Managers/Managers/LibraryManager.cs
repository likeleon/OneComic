using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Exceptions;
using OneComic.Business.Contracts;
using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;

namespace OneComic.Business.Managers.Managers
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public sealed class LibraryManager : ILibraryService
    {
#pragma warning disable 0649
        [Import]
        IDataRepositoryFactory _dataRepositoryFactory;
#pragma warning restore 0649

        public LibraryManager()
        {
            Global.Container.SatisfyImportsOnce(this);
        }

        public Comic[] GetAllComics()
        {
            try
            {
                var comicRepository = _dataRepositoryFactory.GetDataRepository<IComicRepository>();
                return comicRepository.Get().ToArray();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public Comic GetComic(int comicId)
        {
            try
            {
                var comicRepository = _dataRepositoryFactory.GetDataRepository<IComicRepository>();
                var comic = comicRepository.Get(comicId);
                if (comic == null)
                {
                    var ex = new NotFoundException($"Comic with ID of {comicId} is not in database");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                return comic;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}

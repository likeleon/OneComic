using Core.Common.Core;
using Core.Common.Exceptions;
using System;
using System.ComponentModel.Composition;
using System.ServiceModel;

namespace OneComic.Business.Managers
{
    public abstract class Manager
    {
        protected Manager()
        {
            Global.Container?.SatisfyImportsOnce(this);
        }

        protected T ExecuteFaultHandledOperation<T>(Func<T> func)
        {
            try
            {
                return func.Invoke();
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

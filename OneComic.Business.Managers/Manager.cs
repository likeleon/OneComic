using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Exceptions;
using Core.Common.Extensions;
using OneComic.Business.Entities;
using OneComic.Common;
using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Threading;

namespace OneComic.Business.Managers
{
    public abstract class Manager
    {
        protected Manager()
        {
            var context = OperationContext.Current;
            if (context != null)
            {
                try
                {
                    LoginEmail = context.IncomingMessageHeaders.GetHeader<string>("String", "System");
                    if (LoginEmail.Contains(@"\"))
                        LoginEmail = string.Empty;
                }
                catch
                {
                    LoginEmail = string.Empty;
                }
            }

            Global.Container?.SatisfyImportsOnce(this);

            if (!LoginEmail.IsNullOrWhiteSpace())
                AuthorizedAccount = AuthorizeAccount(LoginEmail);
        }

        protected string LoginEmail { get; }
        protected Account AuthorizedAccount { get; }

        protected virtual Account AuthorizeAccount(string loginEmail)
        {
            return null;
        }

        protected void ValidateAuthorization(IAccountOwnedEntity entity)
        {
            if (Thread.CurrentPrincipal.IsInRole(Security.OneComicAdminRole))
                return;

            if (AuthorizedAccount == null)
                return;

            if (LoginEmail.IsNullOrEmpty() || entity.OwnerAccountId == AuthorizedAccount.AccountId)
                return;

            var ex = new AuthorizationValidationException("Attempt to access a secure record with improper user authorization validation.");
            throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
        }

        protected T ExecuteFaultHandledOperation<T>(Func<T> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (AuthorizationValidationException ex)
            {
                throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        protected void ExecuteFaultHandledOperation(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (AuthorizationValidationException ex)
            {
                throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
            }
            catch (FaultException)
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

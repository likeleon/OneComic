using Core.Common.Contracts;
using OneComic.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security;
using System.ServiceModel;
using System.Web.Http;

namespace OneComic.Web.Core
{
    public class ApiControllerBase : ApiController, IServiceAwareController
    {
        private readonly Lazy<List<IServiceContract>> _disposableServices = new Lazy<List<IServiceContract>>(() => new List<IServiceContract>());

        public List<IServiceContract> DisposableServices => _disposableServices.Value;

        void IServiceAwareController.RegisterDisposableServices(List<IServiceContract> disposableServices)
        {
            RegisterServices(disposableServices);
        }

        protected virtual void RegisterServices(List<IServiceContract> disposableServices)
        {
        }

        protected void ValidateAuthroizedUser(string userRequested)
        {
            string userLoggedIn = User.Identity.Name;
            if (userLoggedIn != userRequested)
                throw new SecurityException("Attempting to access data for another user.");
        }

        protected HttpResponseMessage GetHttpResponse(HttpRequestMessage request, Func<HttpResponseMessage> func)
        {
            HttpResponseMessage response = null;

            try
            {
                response = func.Invoke();
            }
            catch (SecurityException ex)
            {
                response = request.CreateResponse(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (FaultException<AuthorizationValidationException> ex)
            {
                response = request.CreateResponse(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (FaultException ex)
            {
                response = request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                response = request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }
    }
}
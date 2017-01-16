using OneComic.Web.Core;
using OneComic.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OneComic.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/account")]
    public sealed class AccountApiController : ApiControllerBase
    {
        private readonly ISecurityAdapter _securityAdapter;

        [ImportingConstructor]
        public AccountApiController(ISecurityAdapter securityAdapter)
        {
            _securityAdapter = securityAdapter;
        }

        [HttpPost]
        [Route("login")]
        public HttpResponseMessage Login(HttpRequestMessage request, [FromBody]AccountLoginModel accountModel)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                bool success = _securityAdapter.Login(accountModel.LoginEmail, accountModel.Password, accountModel.RememberMe);

                if (success)
                    response = request.CreateResponse(HttpStatusCode.OK);
                else
                    response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized login.");

                return response;
            });
        }

        [HttpPost]
        [Route("register/validate1")]
        public HttpResponseMessage ValidateRegistrationStep1(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, () =>
            {
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(accountModel.FirstName))
                    errors.Add("First name is required");

                if (string.IsNullOrWhiteSpace(accountModel.LastName))
                    errors.Add("Last name is required");

                return CreateResponseMessage(request, errors);
            });
        }

        private static HttpResponseMessage CreateResponseMessage(HttpRequestMessage request, List<string> errors)
        {
            if (errors.Count == 0)
                return request.CreateResponse(HttpStatusCode.OK);
            else
                return request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());
        }

        [HttpPost]
        [Route("register/validate2")]
        public HttpResponseMessage ValidateRegistrationStep2(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, () =>
            {
                var errors = new List<string>();

                if (_securityAdapter.UserExists(accountModel.LoginEmail))
                    errors.Add("An account is already registered with this email address.");

                if (accountModel.Password == null || accountModel.Password.Length < 6)
                    errors.Add("Password must be at least 6 characters");

                return CreateResponseMessage(request, errors);
            });
        }

        [HttpPost]
        [Route("register")]
        public HttpResponseMessage CreateAccount(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, () =>
            {
                var response = ValidateRegistrationStep1(request, accountModel);
                if (!response.IsSuccessStatusCode)
                    return response;

                response = ValidateRegistrationStep2(request, accountModel);
                if (!response.IsSuccessStatusCode)
                    return response;

                _securityAdapter.Register(accountModel.LoginEmail, accountModel.Password,
                    propertyValues: new
                    {
                        FirstName = accountModel.FirstName,
                        LastName = accountModel.LastName,
                    });
                _securityAdapter.Login(accountModel.LoginEmail, accountModel.Password, rememberMe: false);

                return request.CreateResponse(HttpStatusCode.OK);
            });
        }
    }
}

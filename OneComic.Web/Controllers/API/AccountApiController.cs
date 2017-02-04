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
                bool success = _securityAdapter.Login(accountModel.LoginEmail, accountModel.Password, accountModel.RememberMe);

                if (success)
                    return request.CreateResponse(HttpStatusCode.OK);
                else
                    return request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized login.");
            });
        }

        [HttpPost]
        [Route("register")]
        public HttpResponseMessage CreateAccount(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, () =>
            {
                var errors = new List<string>();

                if (_securityAdapter.UserExists(accountModel.LoginEmail))
                    errors.Add("An account is already registered with this email address.");

                if (accountModel.Password == null || accountModel.Password.Length < 6)
                    errors.Add("Password must be at least 6 characters");

                if (errors.Count > 0)
                    return request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());

                _securityAdapter.Register(accountModel.LoginEmail, accountModel.Password, null);
                _securityAdapter.Login(accountModel.LoginEmail, accountModel.Password, rememberMe: false);

                return request.CreateResponse(HttpStatusCode.OK);
            });
        }
    }
}

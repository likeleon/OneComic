using OneComic.Web.Core;
using OneComic.Web.Models;
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
    }
}

using OneComic.Web.Core;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OneComic.Web.Controllers.API
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/comics")]
    public sealed class ComicsApiController : ApiControllerBase
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetComics(HttpRequestMessage request)
        {
            return request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Not yet implemented");
        }
    }
}

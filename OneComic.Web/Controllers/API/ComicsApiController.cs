using Core.Common.Contracts;
using OneComic.Client.Contracts;
using OneComic.Web.Core;
using System.Collections.Generic;
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
        private readonly ILibraryService _libraryService;

        [ImportingConstructor]
        public ComicsApiController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        protected override void RegisterServices(List<IServiceContract> disposableServices)
        {
            disposableServices.Add(_libraryService);
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetComics(HttpRequestMessage request)
        {
            return GetHttpResponse(request, () =>
            {
                var comics = _libraryService.GetAllComics();
                return request.CreateResponse(HttpStatusCode.OK, comics);
            });
        }
    }
}

using Core.Common.Contracts;
using Marvin.JsonPatch;
using Newtonsoft.Json;
using OneComic.API.ActionFilters;
using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace OneComic.API.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/comics")]
    public class ComicsController : ApiController
    {
        public const int MaxPageSize = 50;

        private const string GetComicsRouteName = "ComicsList";

        private readonly IComicRepository _repository;
        private readonly IComicMapper _mapper;

        [ImportingConstructor]
        public ComicsController(IComicRepository comicRepository, IComicMapper comicMapper)
        {
            _repository = comicRepository;
            _mapper = comicMapper;
        }

        [Route("", Name = GetComicsRouteName)]
        [HttpGet]
        [FieldsParameter("fields", typeof(Data.DTO.Comic))]
        [PageParameters("page", "pageSize", MaxPageSize)]
        public IHttpActionResult Get(
            string[] fields,
            string sort = "comicId",
            int page = 1,
            int pageSize = MaxPageSize)
        {
            var pagedComics = _repository.Get(sort, page, pageSize);

            var header = CreatePaginationHeader(GetComicsRouteName, pagedComics, sort);
            HttpContext.Current.Response.Headers.Add("X-Pagination", header);

            var comics = pagedComics.Entities.Select(comic => _mapper.ToDataShapedObject(comic, fields));
            return Ok(comics);
        }

        private string CreatePaginationHeader(string routeName, DataPage<Comic> page, string sort)
        {
            var urlHelper = new UrlHelper(Request);

            var prevLink = string.Empty;
            if (page.CurrentPage > 1)
                prevLink = urlHelper.Link(routeName, new { page = page.CurrentPage - 1, pageSize = page.PageSize, sort = sort });

            var nextLink = string.Empty;
            if (page.CurrentPage < page.TotalPages)
                nextLink = urlHelper.Link(routeName, new { page = page.CurrentPage + 1, pageSize = page.PageSize, sort = sort });

            var header = new
            {
                currentPage = page.CurrentPage,
                pageSize = page.PageSize,
                totalCount = page.TotalCount,
                previousPageLink = prevLink,
                nextPageLink = nextLink
            };
            return JsonConvert.SerializeObject(header);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var comic = _repository.Get(id);
            if (comic == null)
                return NotFound();

            return Ok(_mapper.ToDTO(comic));
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Post([FromBody][Required]Data.DTO.Comic comicDto)
        {
            var result = _repository.Add(_mapper.ToEntity(comicDto));
            if (result.State != RepositoryActionState.Created)
                return BadRequest();

            var locationUri = new Uri(Request.RequestUri, result.Entity.ComicId.ToString());
            return Created(locationUri, _mapper.ToDTO(result.Entity));
        }

        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Put(
            int id, 
            [FromBody][Required]Data.DTO.Comic comicDto)
        {
            var result = _repository.Update(_mapper.ToEntity(comicDto));
            switch (result.State)
            {
                case RepositoryActionState.Updated:
                    return Ok(_mapper.ToDTO(result.Entity));
                case RepositoryActionState.NotFound:
                    return NotFound();
                default:
                    return BadRequest();
            }
        }

        [Route("{id}")]
        [HttpPatch]
        public IHttpActionResult Patch(
            int id, 
            [FromBody][Required]JsonPatchDocument<Data.DTO.Comic> comicPatchDocument)
        {
            var comic = _repository.Get(id);
            if (comic == null)
                return NotFound();

            var comicDto = _mapper.ToDTO(comic);
            comicPatchDocument.ApplyTo(comicDto);

            var result = _repository.Update(_mapper.ToEntity(comicDto));
            if (result.State != RepositoryActionState.Updated)
                return BadRequest();

            return Ok(_mapper.ToDTO(result.Entity));
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var result = _repository.Remove(id);
            switch (result.State)
            {
                case RepositoryActionState.Deleted:
                    return StatusCode(HttpStatusCode.NoContent);
                case RepositoryActionState.NotFound:
                    return NotFound();
                default:
                    return BadRequest();
            }
        }
    }
}

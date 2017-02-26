using Core.Common.Contracts;
using Marvin.JsonPatch;
using Newtonsoft.Json;
using OneComic.API.ActionFilters;
using OneComic.API.ModelBinders;
using OneComic.Data.Contracts;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace OneComic.API.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/comics")]
    public class ComicsController : ApiController
    {
        private const int MaxPageSize = 50;
        private const string ComicsListName = "ComicsList";

        private readonly IComicRepository _repository;
        private readonly IComicMapper _mapper;

        [ImportingConstructor]
        public ComicsController(IComicRepository comicRepository, IComicMapper comicMapper)
        {
            _repository = comicRepository;
            _mapper = comicMapper;
        }

        [Route("", Name = ComicsListName)]
        [HttpGet]
        [PageParameters("page", "pageSize", MaxPageSize)]
        public IHttpActionResult Get(
            [ModelBinder(typeof(ComicFieldParamsModelBinder))] FieldParams<Data.DTO.Comic> fields,
            string sort = "comicId",
            int page = 1,
            int pageSize = MaxPageSize)
        {
            var pagedComics = _repository.Get(sort, page, pageSize);

            HttpContext.Current.Response.AddPaginationHeader(Request, ComicsListName, pagedComics, sort);

            var comics = _mapper.ToDataShapedObjects(pagedComics.Entities, fields?.Fields);
            return Ok(comics);
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

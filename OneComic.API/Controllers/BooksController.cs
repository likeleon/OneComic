using Marvin.JsonPatch;
using OneComic.API.ActionFilters;
using OneComic.Data.Entities;
using OneComic.Data;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace OneComic.API.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api")]
    public class BooksController : ApiController
    {
        private const int MaxPageSize = 50;
        private const string BooksForComicName = "BooksForComic";

        private readonly IBookRepository _repository;
        private readonly IBookMapper _mapper;

        [ImportingConstructor]
        public BooksController(IBookRepository bookRepository, IBookMapper bookMapper)
        {
            _repository = bookRepository;
            _mapper = bookMapper;
        }

        [Route("comics/{comicId}/books", Name = BooksForComicName)]
        [HttpGet]
        [PageParameters("page", "pageSize", MaxPageSize)]
        public IHttpActionResult Get(
            int comicId,
            [ModelBinder] DataFields<Data.DTO.Book> fields,
            string sort = "bookId",
            int page = 1,
            int pageSize = MaxPageSize)
        {
            var pagedBooks = _repository.GetByComicId(comicId, sort, page, pageSize);

            HttpContext.Current.Response.AddPaginationHeader(Request, BooksForComicName, pagedBooks, fields, sort);

            var books = _mapper.ToDataShapedObjects(pagedBooks.Entities, fields);
            return Ok(books);
        }

        [Route("comics/{comicId}/books/{id}")]
        [Route("books/{id}")]
        [HttpGet]
        public IHttpActionResult Get(
            int id,
            [ModelBinder] DataFields<Data.DTO.Book> fields,
            int? comicId = null)
        {
            Book book;
            if (comicId == null)
            {
                book = _repository.Get(id);
            }
            else
            {
                var books = _repository.GetByComicId(comicId.Value);
                book = books?.FirstOrDefault(b => b.BookId == id);
            }

            if (book == null)
                return NotFound();

            return Ok(_mapper.ToDataShapedObject(book, fields));
        }

        [Route("books/{id}")]
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

        [Route("books")]
        [HttpPost]
        public IHttpActionResult Post([FromBody][Required]Data.DTO.Book bookDto)
        {
            var book = _mapper.ToEntity(bookDto);
            var result = _repository.Add(book);
            if (result.State != RepositoryActionState.Created)
                return BadRequest();

            var locationUri = new Uri(Request.RequestUri, result.Entity.BookId.ToString());
            return Created(locationUri, _mapper.ToDTO(result.Entity));
        }

        [Route("books/{id}")]
        [HttpPut]
        public IHttpActionResult Put(
            int id, 
            [FromBody][Required]Data.DTO.Book bookDto)
        {
            var book = _mapper.ToEntity(bookDto);
            var result = _repository.Update(book);
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

        [Route("books/{id}")]
        [HttpPatch]
        public IHttpActionResult Patch(
            int id, 
            [FromBody][Required]JsonPatchDocument<Data.DTO.Book> bookPatchDocument)
        {
            var book = _repository.Get(id);
            if (book == null)
                return NotFound();

            var bookDto = _mapper.ToDTO(book);
            bookPatchDocument.ApplyTo(bookDto);

            var result = _repository.Update(_mapper.ToEntity(bookDto));
            if (result.State != RepositoryActionState.Updated)
                return BadRequest();

            return Ok(_mapper.ToDTO(result.Entity));
        }
    }
}

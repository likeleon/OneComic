using Core.Common.Contracts;
using Marvin.JsonPatch;
using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace OneComic.API.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api")]
    public class BooksController : ApiController
    {
        private readonly IBookRepository _repository;
        private readonly IBookMapper _mapper;

        [ImportingConstructor]
        public BooksController(IBookRepository bookRepository, IBookMapper bookMapper)
        {
            _repository = bookRepository;
            _mapper = bookMapper;
        }

        [Route("comics/{comicId}/books")]
        [HttpGet]
        public IHttpActionResult Get(int comicId)
        {
            var books = _repository.GetByComicId(comicId);
            if (books == null)
                return NotFound();

            return Ok(books.Select(_mapper.ToDTO));
        }

        [Route("comics/{comicId}/books/{id}")]
        [Route("books/{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id, int? comicId = null)
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

            return Ok(_mapper.ToDTO(book));
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

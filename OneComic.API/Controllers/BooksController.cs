using Core.Common.Contracts;
using Marvin.JsonPatch;
using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System;
using System.ComponentModel.Composition;
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
            try
            {
                var books = _repository.GetByComicId(comicId);
                if (books == null)
                    return NotFound();

                return Ok(books.Select(_mapper.ToDTO));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("comics/{comicId}/books/{id}")]
        [Route("books/{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id, int? comicId = null)
        {
            try
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
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("books/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
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
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("books")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Data.DTO.Book bookDto)
        {
            try
            {
                if (bookDto == null)
                    return BadRequest();

                var book = _mapper.ToEntity(bookDto);
                var result = _repository.Add(book);
                if (result.State != RepositoryActionState.Created)
                    return BadRequest();

                var locationUri = new Uri(Request.RequestUri, result.Entity.BookId.ToString());
                return Created(locationUri, _mapper.ToDTO(result.Entity));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("books/{id}")]
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]Data.DTO.Book bookDto)
        {
            try
            {
                if (bookDto == null)
                    return BadRequest();

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
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("books/{id}")]
        [HttpPatch]
        public IHttpActionResult Patch(int id, [FromBody]JsonPatchDocument<Data.DTO.Book> bookPatchDocument)
        {
            try
            {
                if (bookPatchDocument == null)
                    return BadRequest();

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
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}

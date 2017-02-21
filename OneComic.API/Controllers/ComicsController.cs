using OneComic.Data.Contracts;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Http;

namespace OneComic.API.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ComicsController : ApiController
    {
        private readonly IComicRepository _repository;
        private readonly IComicMapper _mapper;

        [ImportingConstructor]
        public ComicsController(IComicRepository comicRepository, IComicMapper comicMapper)
        {
            _repository = comicRepository;
            _mapper = comicMapper;
        }

        public IHttpActionResult Get()
        {
            try
            {
                var comics = _repository.Get().Select(_mapper.ToDTO);
                return Ok(comics);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Get(int id)
        {
            try
            {
                var comic = _repository.Get(id);
                if (comic == null)
                    return NotFound();

                return Ok(_mapper.ToDTO(comic));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}

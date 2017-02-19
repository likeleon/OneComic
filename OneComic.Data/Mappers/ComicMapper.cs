using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.ComponentModel.Composition;

namespace OneComic.Data.Mappers
{
    [Export(typeof(IComicMapper))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class ComicMapper : IComicMapper
    {
        public DTO.Comic ToDTO(Comic comic)
        {
            return new DTO.Comic
            {
                ComicId = comic.ComicId,
                Title = comic.Title
            };
        }

        public Comic ToEntity(DTO.Comic comic)
        {
            return new Comic
            {
                ComicId = comic.ComicId,
                Title = comic.Title
            };
        }
    }
}

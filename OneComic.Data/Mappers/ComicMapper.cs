using OneComic.Business.Entities;
using System;
using System.ComponentModel.Composition;

namespace OneComic.Data
{
    [Export(typeof(IComicMapper))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class ComicMapper : DataMapper<Comic, DTO.Comic>, IComicMapper
    {
        private readonly IBookMapper _bookMapper;

        [ImportingConstructor]
        public ComicMapper(IBookMapper bookMapper)
        {
            _bookMapper = bookMapper;
        }

        public override DTO.Comic ToDTO(Comic comic)
        {
            return new DTO.Comic
            {
                ComicId = comic.ComicId,
                CoverImageUri = new Uri(comic.CoverImageUri),
                Title = comic.Title
            };
        }

        public override Comic ToEntity(DTO.Comic comic)
        {
            return new Comic
            {
                ComicId = comic.ComicId,
                CoverImageUri = comic.CoverImageUri.AbsoluteUri,
                Title = comic.Title
            };
        }

        protected override object CreateAssociatedObject(
            Comic comic, 
            string associationName, 
            IDataFields fields)
        {
            if (associationName == nameof(Comic.Books))
                return _bookMapper.ToDataShapedObjects(comic.Books, fields);

            throw new Exception($"Unknown association: {associationName}");
        }
    }
}

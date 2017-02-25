using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace OneComic.Data.Mappers
{
    [Export(typeof(IComicMapper))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class ComicMapper : IComicMapper
    {
        private readonly IReadOnlyDictionary<string, PropertyInfo> _propertyInfoByFieldName;

        public ComicMapper()
        {
            _propertyInfoByFieldName = typeof(DTO.Comic).GetDtoProperties()
                .ToDictionary(p => p.Name, p => p, StringComparer.InvariantCultureIgnoreCase);
        }

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

        public object ToDataShapedObject(Comic comic, IEnumerable<string> fields)
        {
            return ToDataShapedObject(ToDTO(comic), fields);
        }

        public object ToDataShapedObject(DTO.Comic comic, IEnumerable<string> fields)
        {
            if (fields?.Any() != true)
                return comic;

            var obj = new ExpandoObject() as IDictionary<string, object>;
            foreach (var field in fields)
            {
                var value = _propertyInfoByFieldName[field].GetValue(comic);
                obj.Add(field, value);
            }

            return obj;
        }
    }
}

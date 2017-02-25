using OneComic.Business.Entities;
using OneComic.Data.Contracts;
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
        private readonly IReadOnlyDictionary<string, PropertyInfo> _propertyInfos;

        public ComicMapper()
        {
            var bindingAttr = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            _propertyInfos = typeof(DTO.Comic).GetProperties(bindingAttr)
                .ToDictionary(p => p.Name, p => p);
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

        public bool HasProperty(string property) => _propertyInfos.ContainsKey(property);

        public object ToDataShapedObject(Comic comic, IEnumerable<string> properties)
        {
            return ToDataShapedObject(ToDTO(comic), properties);
        }

        public object ToDataShapedObject(DTO.Comic comic, IEnumerable<string> properties)
        {
            if (!properties.Any())
                return comic;

            var obj = new ExpandoObject() as IDictionary<string, object>;
            foreach (var property in properties)
            {
                var value = _propertyInfos[property].GetValue(comic);
                obj.Add(property, value);
            }

            return obj;
        }
    }
}

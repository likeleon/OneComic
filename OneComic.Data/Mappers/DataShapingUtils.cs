using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneComic.Data
{
    public static class DataShapingUtils
    {
        public static IEnumerable<string> GetDtoFields(this Type dtoType)
        {
            return GetProperties(dtoType).Select(property => property.Name);
        }

        internal static IReadOnlyDictionary<string, PropertyInfo> GetDtoFieldsWithPropertyInfo(this Type dtoType)
        {
            return GetProperties(dtoType).ToDictionary(property => property.Name);
        }

        private static PropertyInfo[] GetProperties(Type dtoType)
        {
            var bindingAttr = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            return dtoType.GetProperties(bindingAttr);
        }
    }
}

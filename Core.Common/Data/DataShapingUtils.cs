using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Common.Data
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

        public static IEnumerable<string> Flatten(this IDataFields fields)
        {
            foreach (var field in fields.Fields)
                yield return field;

            foreach (var kvp in fields.AssociatedFields)
                yield return $"{kvp.Key}.{kvp.Value}";
        }
    }
}

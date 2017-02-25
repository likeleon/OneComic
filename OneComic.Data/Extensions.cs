using System;
using System.Collections.Generic;
using System.Reflection;

namespace OneComic.Data
{
    public static class Extensions
    {
        public static IEnumerable<PropertyInfo> GetDtoProperties(this Type dtoType)
        {
            var bindingAttr = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            return dtoType.GetProperties(bindingAttr);
        }
    }
}

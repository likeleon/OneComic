using Core.Common.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Common.Core
{
    internal static class ObjectGraph
    {
        private static Cache<Type, PropertyInfo[]> _propertiesByTypeCache = new Cache<Type, PropertyInfo[]>(type => GetProperties(type).ToArray());

        public static IEnumerable<ObjectBase> GetSelfAndDescendants(ObjectBase root)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            yield return root;

            var toVisit = new Stack<ObjectBase>();
            toVisit.Push(root);

            var visited = new HashSet<ObjectBase>();

            do
            {
                var obj = toVisit.Pop();
                visited.Add(obj);

                foreach (var propertyInfo in _propertiesByTypeCache[obj.GetType()])
                {
                    var property = propertyInfo.GetValue(obj, null);
                    if (visited.Contains(property))
                        continue;

                    if (property is ObjectBase)
                    {
                        yield return property as ObjectBase;
                        toVisit.Push(property as ObjectBase);
                        continue;
                    }

                    var enumerable = propertyInfo.GetValue(obj, null) as IEnumerable;
                    if (enumerable == null)
                        continue;

                    foreach (var e in enumerable.OfType<ObjectBase>())
                    {
                        yield return e;
                        toVisit.Push(e);
                    };
                }
            } while (toVisit.Count > 0);
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                    yield return property;

                if (property.PropertyType.GetInterface(nameof(IEnumerable)) != null)
                    yield return property;
            }
        }
    }
}

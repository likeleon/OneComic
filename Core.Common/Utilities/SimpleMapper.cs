using System;
using System.Linq;

namespace Core.Common.Utilities
{
    internal static class SimpleMapper
    {
        public static void PropertyMap<T, U>(T source, U destination)
            where T : class, new()
            where U : class, new()
        {
            var sourceProperties = source.GetType().GetProperties().ToArray();
            var destinationProperties = destination.GetType().GetProperties().ToDictionary(p => p.Name, p => p);
            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.GetValueOrDefault(sourceProperty.Name);
                if (destinationProperty == null)
                    continue;

                try
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                }
                catch (ArgumentException)
                {
                }
            }
        }
    }
}

using OneComic.Core;
using System.Collections.Generic;
using System.Linq;

namespace OneComic.API.Client
{
    public static class UriHelper
    {
        public static Dictionary<string, string> WithFields(
            this Dictionary<string, string> query, 
            IEnumerable<string> fields,
            string key = "fields")
        {
            if (fields?.Any() != true)
                return query;

            query.Add(key, fields.JoinWith(","));
            return query;
        }
    }
}

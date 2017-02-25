using Core.Common.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web.Http;

namespace OneComic.API
{
    public static class WebApiConfig
    {
        private static IEnumerable<MediaTypeHeaderValue> JsonSupportedMediaTypes
        {
            get
            {
                yield return new MediaTypeHeaderValue("application/octet-stream");
                yield return new MediaTypeHeaderValue("application/json-patch+json");
            }
        }

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            JsonSupportedMediaTypes.Do(config.Formatters.JsonFormatter.SupportedMediaTypes.Add);

            var jsonSerializerSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSerializerSettings.Formatting = Formatting.Indented;
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
        }
    }
}

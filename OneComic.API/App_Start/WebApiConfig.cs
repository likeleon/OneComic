using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneComic.API.ActionFilters;
using OneComic.Core;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

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

            AddFilters(config);
            ConfigureFormatters(config);
            EnableCaching(config);
            EnableCors(config);

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
        }

        private static void EnableCaching(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new CacheCow.Server.CachingHandler(config));
        }

        private static void EnableCors(HttpConfiguration config)
        {
            var corsAttribute = new EnableCorsAttribute(
                origins: "*",
                headers: "*",
                methods: "*",
                exposedHeaders: HttpResponseExtensions.PaginationHeaderName);
            config.EnableCors(corsAttribute);
        }

        private static void ConfigureFormatters(HttpConfiguration config)
        {
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            JsonSupportedMediaTypes.Do(config.Formatters.JsonFormatter.SupportedMediaTypes.Add);

            var jsonSerializerSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSerializerSettings.Formatting = Formatting.Indented;
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private static void AddFilters(HttpConfiguration config)
        {
            config.Filters.Add(new ValidateActionParametersAttribute());
            config.Filters.Add(new ValidateModelAttribute());
        }
    }
}

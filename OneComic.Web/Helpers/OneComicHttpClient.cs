using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace OneComic.Web.Helpers
{
    public sealed class OneComicHttpClient
    {
        private static readonly Lazy<HttpClient> _lazy = new Lazy<HttpClient>(Create);

        public static HttpClient Instance => _lazy.Value;

        private static HttpClient Create()
        {
            var client = new HttpClient();
            client.BaseAddress = GetBaseAddress();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private static Uri GetBaseAddress()
        {
            var request = HttpContext.Current.Request;
            return new Uri(request.Url.GetLeftPart(UriPartial.Authority));
        }
    }
}
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OneComic.API.Client
{
    internal static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            var request = CreateRequest(requestUri, content);
            return client.SendAsync(request);
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content)
        {
            var request = CreateRequest(requestUri, content);
            return client.SendAsync(request);
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            var request = CreateRequest(requestUri, content);
            return client.SendAsync(request, cancellationToken);
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            var request = CreateRequest(requestUri, content);
            return client.SendAsync(request, cancellationToken);
        }

        private static HttpRequestMessage CreateRequest(string requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            return new HttpRequestMessage(method, requestUri) { Content = content };
        }

        private static HttpRequestMessage CreateRequest(Uri requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            return new HttpRequestMessage(method, requestUri) { Content = content };
        }
    }
}

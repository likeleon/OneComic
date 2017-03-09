using Marvin.HttpCache;
using OneComic.Core;
using OneComic.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OneComic.API.Client
{
    public sealed class OneComicClient
    {
        private readonly HttpClient _client;

        public OneComicClient(Uri serverBaseAddress)
        {
            _client = new HttpClient(CreateHttpCacheHandler());
            _client.BaseAddress = serverBaseAddress;

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static HttpMessageHandler CreateHttpCacheHandler()
        {
            return new HttpCacheHandler
            {
                InnerHandler = new HttpClientHandler()
            };
        }

        public async Task<Comic[]> GetComics(IEnumerable<string> fields)
        {
            var dictionary = new Dictionary<string, string>();
            if (fields.Any())
                dictionary.Add("fields", fields.JoinWith(","));

            var query = MakeUri("comics", dictionary);
            var response = await _client.GetAsync(query);
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync();
            // TODO
        }

        private static string MakeUri(string path, IReadOnlyDictionary<string, string> query)
        {
            if (!query.Any())
                return path;

            var queryString = query.Select(kvp => $"{kvp.Key}={kvp.Value}").JoinWith("&");
            return $"{path}?{queryString}";
        }
    }
}

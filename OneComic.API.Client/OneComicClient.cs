using Marvin.HttpCache;
using Newtonsoft.Json;
using OneComic.Core;
using OneComic.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OneComic.API.Client
{
    public sealed class OneComicClient
    {
        private readonly HttpClient _client;

        public OneComicClient(string serverBaseAddress)
        {
            _client = new HttpClient(CreateHttpCacheHandler());
            _client.BaseAddress = new Uri(serverBaseAddress);

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

        public async Task<Comic[]> GetComics(IEnumerable<string> fields = null)
        {
            var query = new Dictionary<string, string>().WithFields(fields);
            var requestUri = MakeUri("comics", query);
            var response = await _client.GetAsync(requestUri);
            return await DeserializeResponse<Comic[]>(response);
        }

        public async Task<Comic> AddComic(Comic comic)
        {
            var content = SerializeToJsonContent(comic);
            var response = await _client.PostAsync("comics", content);
            return await DeserializeResponse<Comic>(response);
        }

        public async Task DeleteComic(int comicId)
        {
            var response = await _client.DeleteAsync($"comics/{comicId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<Book[]> GetBooks(int comicId, IEnumerable<string> fields = null)
        {
            var query = new Dictionary<string, string>().WithFields(fields);
            var requestUri = MakeUri($"comics/{comicId}/books", query);
            var response = await _client.GetAsync(requestUri);
            return await DeserializeResponse<Book[]>(response);
        }

        public async Task<Book> AddBook(Book book)
        {
            var content = SerializeToJsonContent(book);
            var response = await _client.PostAsync("books", content);
            return await DeserializeResponse<Book>(response);
        }

        public async Task DeleteBook(int bookId)
        {
            var response = await _client.DeleteAsync($"books/{bookId}");
            response.EnsureSuccessStatusCode();
        }

        private static string MakeUri(string path, IReadOnlyDictionary<string, string> query)
        {
            if (!query.Any())
                return path;

            var queryString = query.Select(kvp => $"{kvp.Key}={kvp.Value}").JoinWith("&");
            return $"{path}?{queryString}";
        }

        private static StringContent SerializeToJsonContent(object obj)
        {
            var jsonContent = JsonConvert.SerializeObject(obj);
            return new StringContent(jsonContent, Encoding.Unicode, "application/json");
        }

        private static async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}

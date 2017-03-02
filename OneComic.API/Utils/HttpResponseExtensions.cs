using Newtonsoft.Json;
using OneComic.Core;
using OneComic.Data;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace OneComic.API
{
    public static class HttpResponseExtensions
    {
        public const string PaginationHeaderName = "X-Pagination";

        public static void AddPaginationHeader<T>(
            this HttpResponse response,
            HttpRequestMessage request,
            string routeName,
            DataPage<T> page,
            IDataFields fields,
            string sort)
        {
            var urlHelper = new UrlHelper(request);

            var fieldsStr = fields?.Flatten()?.JoinWith(",") ?? string.Empty;

            var prevLink = string.Empty;
            if (page.CurrentPage > 1)
            {
                var routeValues = new
                {
                    page = page.CurrentPage - 1,
                    pageSize = page.PageSize,
                    fields = fieldsStr,
                    sort = sort
                };
                prevLink = urlHelper.Link(routeName, routeValues);
            }

            var nextLink = string.Empty;
            if (page.CurrentPage < page.TotalPages)
            {
                var routeValues = new
                {
                    page = page.CurrentPage + 1,
                    pageSize = page.PageSize,
                    fields = fieldsStr,
                    sort = sort
                };
                nextLink = urlHelper.Link(routeName, routeValues);
            }

            var header = JsonConvert.SerializeObject(new
            {
                currentPage = page.CurrentPage,
                pageSize = page.PageSize,
                totalCount = page.TotalCount,
                previousPageLink = prevLink,
                nextPageLink = nextLink
            });
            response.Headers.Add(PaginationHeaderName, header);
        }
    }
}
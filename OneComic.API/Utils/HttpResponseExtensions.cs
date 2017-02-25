using Core.Common.Contracts;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace OneComic.API
{
    public static class HttpResponseExtensions
    {
        public static void AddPaginationHeader<T>(
            this HttpResponse response,
            HttpRequestMessage request,
            string routeName,
            DataPage<T> page,
            string sort)
        {
            var urlHelper = new UrlHelper(request);

            var prevLink = string.Empty;
            if (page.CurrentPage > 1)
                prevLink = urlHelper.Link(routeName, new { page = page.CurrentPage - 1, pageSize = page.PageSize, sort = sort });

            var nextLink = string.Empty;
            if (page.CurrentPage < page.TotalPages)
                nextLink = urlHelper.Link(routeName, new { page = page.CurrentPage + 1, pageSize = page.PageSize, sort = sort });

            var header = JsonConvert.SerializeObject(new
            {
                currentPage = page.CurrentPage,
                pageSize = page.PageSize,
                totalCount = page.TotalCount,
                previousPageLink = prevLink,
                nextPageLink = nextLink
            });
            response.Headers.Add("X-Pagination", header);
        }
    }
}
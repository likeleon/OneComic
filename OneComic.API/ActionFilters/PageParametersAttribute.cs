using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace OneComic.API.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class PageParametersAttribute : ActionFilterAttribute
    {
        public string PageParameterName { get; }
        public string PageSizeParameterName { get; }
        public int? MaxPage { get; set; }

        public PageParametersAttribute(string pageParameterName, string pageSizeParameterName)
        {
            PageParameterName = pageParameterName;
            PageSizeParameterName = pageSizeParameterName;
        }

        public PageParametersAttribute(string pageParameterName, string pageSizeParameterName, int maxPage)
            : this(pageParameterName, pageSizeParameterName)
        {
            MaxPage = maxPage;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var page = (int)actionContext.ActionArguments[PageParameterName];
            if (page <= 0)
                actionContext.ThrowBadRequestResponse($"Page '{PageParameterName}' should be a positive value");

            var pageSize = (int)actionContext.ActionArguments[PageSizeParameterName];
            if (pageSize <= 0)
                actionContext.ThrowBadRequestResponse($"Page size '{PageSizeParameterName}' should be a positive value");

            if (MaxPage.HasValue && pageSize > MaxPage.Value)
                actionContext.ThrowBadRequestResponse($"Page size '{PageSizeParameterName}' should be less than {MaxPage.Value}");
        }
    }
}
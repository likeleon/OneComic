using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace OneComic.API.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, 
                    actionContext.ModelState);
                throw new HttpResponseException(response);
            }
        }
    }
}
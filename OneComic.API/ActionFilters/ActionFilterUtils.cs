using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace OneComic.API.ActionFilters
{
    public static class ActionFilterUtils
    {
        public static void ThrowBadRequestResponse(this HttpActionContext actionContext, string message)
        {
            var response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            throw new HttpResponseException(response);
        }
    }
}
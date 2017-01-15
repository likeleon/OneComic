using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace OneComic.Web.Core
{
    public sealed class UserDisposableServiceAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as IServiceAwareController;
            controller?.RegisterDisposableServices(controller.DisposableServices);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as IServiceAwareController;
            foreach (var service in controller?.DisposableServices)
                (service as IDisposable)?.Dispose();
        }
    }
}
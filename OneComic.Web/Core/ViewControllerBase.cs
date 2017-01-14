using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OneComic.Web.Core
{
    public class ViewControllerBase : Controller
    {
        private readonly Lazy<List<IServiceContract>> _disposableServices = new Lazy<List<IServiceContract>>(() => new List<IServiceContract>());

        public List<IServiceContract> DisposableServices => _disposableServices.Value;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            RegisterServices(DisposableServices);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            foreach (var service in DisposableServices.OfType<IDisposable>())
                service.Dispose();
        }

        protected virtual void RegisterServices(List<IServiceContract> disposableServices)
        {
        }
    }
}
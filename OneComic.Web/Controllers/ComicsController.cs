using OneComic.Web.Core;
using System.ComponentModel.Composition;
using System.Web.Mvc;

namespace OneComic.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("comics")]
    public class ComicsController : ViewControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
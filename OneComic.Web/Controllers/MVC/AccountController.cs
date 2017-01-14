using OneComic.Web.Core;
using OneComic.Web.Models;
using System.ComponentModel.Composition;
using System.Web.Mvc;

namespace OneComic.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("account")]
    public sealed class AccountController : ViewControllerBase
    {
        private readonly ISecurityAdapter _securityAdapter;

        [ImportingConstructor]
        public AccountController(ISecurityAdapter securityAdapter)
        {
            _securityAdapter = securityAdapter;
        }

        [HttpGet]
        [Route("login")]
        public ActionResult Login(string returnUrl)
        {
            _securityAdapter.Initialize();
            return View(new AccountLoginModel { ReturnUrl = returnUrl });
        }
    }
}
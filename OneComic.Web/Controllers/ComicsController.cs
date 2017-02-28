using Newtonsoft.Json;
using OneComic.Data.DTO;
using OneComic.Web.Core;
using OneComic.Web.Helpers;
using OneComic.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OneComic.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("comics")]
    public class ComicsController : ViewControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var client = OneComicHttpClient.Instance;

            var response = await client.GetAsync("api/comics");
            if (!response.IsSuccessStatusCode)
                return Content("An error occured.");

            var content = await response.Content.ReadAsStringAsync();
            var comics = JsonConvert.DeserializeObject<IEnumerable<Comic>>(content);
            return View(new ComicsViewModel { Comics = comics });
        }
    }
}
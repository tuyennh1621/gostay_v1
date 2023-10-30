using Microsoft.AspNetCore.Mvc;

namespace GoStay.Web.Controllers
{
    public class MapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

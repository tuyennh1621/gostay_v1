using Microsoft.AspNetCore.Mvc;

namespace goStayCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DocumentationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

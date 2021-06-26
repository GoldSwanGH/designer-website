using Microsoft.AspNetCore.Mvc;

namespace designer_website.Controllers
{
    public class EssentialController : Controller
    {
        public IActionResult Authors()
        {
            return View();
        }

        public IActionResult Author()
        {
            return View();
        }
    }
}
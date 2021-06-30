using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using designer_website.Models;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly MSDBcontext dbcontext;
        public HomeController(ILogger<HomeController> logger, MSDBcontext dbcontext)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return Redirect("Index#info");
        }
        
        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
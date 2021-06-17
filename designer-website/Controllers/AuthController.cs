using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace designer_website.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly MSDBcontext _dbcontext;

        public AuthController(MSDBcontext dbcontext, ILogger<AuthController> logger)
        {
            this._dbcontext = dbcontext;
            this._logger = logger;
        }
        // GET
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
using designer_website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BC = BCrypt.Net.BCrypt;

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
        [HttpPost]
        public IActionResult CreateUser()
        {
            int count = Request.Form.Count;
            ViewData["Count"] = count;
            
            string email = Request.Form["email"];
            string password = BC.HashPassword((string)Request.Form["password"]);
            string firstName = Request.Form["firstname"];
            
            User newUser = new User();
            newUser.Email = email;
            newUser.Password = password;
            newUser.FirstName = firstName;
            _dbcontext.Users.Add(newUser);
            _dbcontext.SaveChanges();
            
            ViewData["Email"] = email;
            ViewData["FirstName"] = firstName;
            
            return View();
        }
    }
}
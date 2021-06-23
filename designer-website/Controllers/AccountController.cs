using designer_website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BC = BCrypt.Net.BCrypt;

namespace designer_website.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly MSDBcontext _dbcontext;

        public AccountController(MSDBcontext dbcontext, ILogger<AccountController> logger)
        {
            this._dbcontext = dbcontext;
            this._logger = logger;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(UserViewModel userViewModel)
        {

            if (ModelState.IsValid) 
            {
                _dbcontext.Users.Add(userViewModel.ToUser());
                _dbcontext.SaveChanges();
                return RedirectToAction("UserCreated");
            }   
            
            return View(userViewModel); // Если валидация не прошла, возвращаемся на страницу регистрации.
        }

        public IActionResult EmailConfirmation()
        {
            return View();
        }

        public IActionResult UserCreated()
        {
            return View();
        }
    }
}
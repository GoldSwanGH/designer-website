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
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserViewModel userViewModel)
        {

            if (ModelState.IsValid)
            {
                _dbcontext.Users.Add(userViewModel.ToUser());
                _dbcontext.SaveChanges();
                return RedirectToAction("UserCreated");
            }   
            
            return View(userViewModel); // Если валидация не прошла, возвращаемся на страницу регистрации.
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(UserViewModel userViewModel)
        {
            return View();
        }

        public IActionResult EmailConfirmation()
        {
            /* Сделать настраиваемый текст и ссылку, чтобы использовать эту страницу как для подтверждения регистрации,
               так и для смены пароля */
            ViewData["Text"] = "Check your email and follow the link to continue.";
            return View();
        }

        public IActionResult UserCreated()
        {
            return View();
        }
    }
}
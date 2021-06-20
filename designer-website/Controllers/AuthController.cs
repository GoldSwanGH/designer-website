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
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(UserViewModel userViewModel)
        {
            // старый код
            /*
            int count = Request.Form.Count;
            ViewData["Count"] = count;
            
            string email = Request.Form["email"];
            // если есть такой пользователь - возвращаем пользователя на SignIn с подписью, что User с таким Email 
            // уже существует
            
            string password = BC.HashPassword((string)Request.Form["password"]);
            string firstName = Request.Form["firstname"];
            
            User newUser = new User();
            newUser.Email = email;
            newUser.Password = password;
            newUser.FirstName = firstName;
            _dbcontext.Users.Add(newUser);
            _dbcontext.SaveChanges();
            
            ViewData["Email"] = email;
            ViewData["FirstName"] = firstName; */
            
            // новый код
            
            /* Валидация происходит либо на стороне клиента, либо на сервере через ModelState.IsValid, если у клиента
             отключен JavaScript. Правила валидации описаны в моделях. */
            
            if (ModelState.IsValid) 
            {
                _dbcontext.Users.Add(userViewModel.ToUser());
                _dbcontext.SaveChanges();
                return RedirectToAction("UserCreated");
            }   
            
            return View(userViewModel); // Если валидация не прошла, возвращаемся на страницу регистрации.
        }

        public IActionResult UserCreated()
        {
            return View();
        }
    }
}
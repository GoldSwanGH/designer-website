using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using designer_website.Filters;
using designer_website.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [AnonymousOnlyFilter]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AnonymousOnlyFilter]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            if (ModelState.IsValid)
            {
                User user = registerViewModel.ToUser();
                Role userRole = await _dbcontext.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
                _dbcontext.Users.Add(user);

                if (userRole != null)
                {
                    user.Role = userRole;
                }
                else
                {
                    return View(registerViewModel);
                }

                await _dbcontext.SaveChangesAsync();

                await Authenticate(user);
                
                return RedirectToAction("UserCreated");
            }   
            
            return View(registerViewModel); // Если валидация не прошла, возвращаемся на страницу регистрации.
        }
        
        [HttpGet]
        [AnonymousOnlyFilter]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [AnonymousOnlyFilter]
        public async Task<IActionResult> Login(LoginViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _dbcontext.Users
                    .Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == userViewModel.Email);
                if (user != null && BC.Verify(userViewModel.Password, user.Password))
                {
                    await Authenticate(user);
 
                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult EmailConfirmation()
        {
            /* Сделать настраиваемый текст и ссылку, чтобы использовать эту страницу как для подтверждения регистрации,
               так и для смены пароля */
            ViewData["Text"] = "Check your email and follow the link to continue.";
            return View();
        }

        [Authorize]
        public IActionResult UserCreated()
        {
            return View();
        }
        
        [Authorize]
        public IActionResult Manage()
        {
            return View();
        }
        
        [Authorize]
        public IActionResult Orders()
        {
            return View();
        }
        
        [Authorize(Roles = "Designer")]
        public IActionResult Works()
        {
            return View();
        }

        /* Default admin account
         
        public IActionResult CreateAdmin()
        {
            User admin = new User
            {
                Email = "admin@gg",
                FirstName = "admin",
                Password = BC.HashPassword("admin12345"),
                Role = _dbcontext.Roles.FirstOrDefault(r => r.RoleName == "Admin"),
                Tel = "1"
            };
            
            _dbcontext.Users.Add(admin);
            _dbcontext.SaveChanges();
            
            return RedirectToAction("Index", "Home");
        }
        
        Default designer account
        
        public IActionResult CreateDesigner()
        {
            User designer = new User
            {
                Email = "designer@gg",
                FirstName = "designer",
                Password = BC.HashPassword("designer12345"),
                Role = _dbcontext.Roles.FirstOrDefault(r => r.RoleName == "Designer"),
                Tel = "2"
            };
            
            _dbcontext.Users.Add(designer);
            _dbcontext.SaveChanges();
            
            return RedirectToAction("Index", "Home");
        }
        */
        
        private async Task Authenticate(User user)
        {
            // создаем claims
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.RoleName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
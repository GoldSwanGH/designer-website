﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using designer_website.Filters;
using designer_website.Interfaces;
using designer_website.Models;
using designer_website.Models.EntityFrameworkModels;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Org.BouncyCastle.Crypto.Digests;
using BC = BCrypt.Net.BCrypt;

namespace designer_website.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly MSDBcontext _dbcontext;
        private readonly ISmtpEmailSender _emailSender;
        private readonly ITokenizer _tokenizer;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(MSDBcontext dbcontext, ILogger<AccountController> logger, ISmtpEmailSender emailSender,
            ITokenizer tokenizer, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = dbcontext;
            _logger = logger;
            _emailSender = emailSender;
            _tokenizer = tokenizer;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
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

                if (userRole != null)
                {
                    user.Role = userRole;
                }
                else
                {
                    return View(registerViewModel);
                }

                return RedirectToAction("EmailConfirmation", user);
            }   
            
            return View(registerViewModel); // Если валидация не прошла, возвращаемся на страницу регистрации.
        }
        
        public IActionResult EmailConfirmation(User user)
        {
            User sameUser = _dbcontext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (sameUser == null)
            {
                string token = _tokenizer.GetRandomToken();
                string url = "https://localhost:44357/Account/EmailConfirmation/" + token;
                user.Token = token;

                var to = new MailboxAddress(user.FirstName, user.Email);
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<p>Пройдите по ссылке, чтобы подтвердить регистрацию:</p><br /><a href=\"" 
                                       + url + "\">" + url + "</a>";
                bodyBuilder.TextBody = "Пройдите по ссылке, чтобы подтвердить регистрацию:\n" + url;
            
                var sendEmail = _emailSender.TryToSendMail(to, "Подтверждение регистрации", bodyBuilder.ToMessageBody());

                if (sendEmail == EmailResult.SendSuccess)
                {
                    if (user.Role == _dbcontext.Roles.FirstOrDefault(r => r.RoleName == "Designer"))
                    {
                        ViewData["Text"] = "Аккаунт дизайнера был создан, письмо с подтверждением регистрации было " + 
                                           "отправлено на почту дизайнера.";
                    }
                    else
                    {
                        ViewData["Text"] = "Ваш аккаунт был создан, чтобы подтвердить регистрацию и активировать аккаунт, " +
                                           "пройдите по ссылке из письма, которое мы отправили Вам на почту.";
                    }
                    
                    _dbcontext.Users.Add(user);
                    _dbcontext.SaveChanges();
                }
                else
                {
                    ViewData["Text"] = "Ошибка при отправке письма, проверьте введенную Вами при регистрации почту " + 
                                       "и попробуйте пройти регистрацию еще раз.";
                }
            }
            else
            {
                ViewData["Text"] = "Ошибка при отправке письма.";
            }
            return View();
        }
        
        [Route("Account/EmailConfirmation/{token}")]
        public async Task<IActionResult> EmailConfirmation(string token)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Token == token);
            
            if (user != null)
            {
                user.Token = null;
                user.EmailConfirmed = true;
                await _dbcontext.SaveChangesAsync();
                
                return RedirectToAction("UserCreated");
            }
            
            ViewData["Text"] = "Ошибка. Скорее всего, Ваша ссылка нерабочая. Попробуйте зарегистрироваться снова.";
            
            return View();
        }
        
        [HttpGet]
        [AnonymousOnlyFilter]
        public IActionResult Recovery()
        {
            ViewData["Post"] = false;
            return View();
        }

        [HttpPost]
        public IActionResult Recovery(UserViewModel recoveryViewModel)
        {
            ViewData["Post"] = true;
            User sameUser = _dbcontext.Users.FirstOrDefault(u => u.Email == recoveryViewModel.Email);
            if (sameUser != null)
            {
                string token = _tokenizer.GetRandomToken();
                string url = "https://localhost:44357/Account/PasswordChange/" + token;

                var to = new MailboxAddress(sameUser.FirstName, sameUser.Email);
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<p>Пройдите по ссылке, чтобы изменить пароль:</p><br /><a href=\"" 
                                       + url + "\">" + url + "</a>";
                bodyBuilder.TextBody = "Пройдите по ссылке, чтобы изменить пароль:\n" + url;
            
                var sendEmail = _emailSender.TryToSendMail(to, "Изменение пароля", bodyBuilder.ToMessageBody());

                if (sendEmail == EmailResult.SendSuccess)
                {
                    sameUser.Token = token;
                    _dbcontext.SaveChanges();
                }
            }
            return View();
        }
        
        [HttpGet]
        [Route("Account/PasswordChange/{token}")]
        [AnonymousOnlyFilter]
        public IActionResult PasswordChange(string token)
        {
            var user = _dbcontext.Users.FirstOrDefault(u => u.Token == token);
            
            if (user == null)
            {
                ViewData["Post"] = true;
                ViewData["Text"] = "Ошибка. Неверная ссылка.";
            }
            else
            {
                ViewData["Post"] = false;
                
                user.Token = null;
                _dbcontext.SaveChanges();
            }

            var passwordChangeViewModel = new PasswordChangeViewModel();
            passwordChangeViewModel.Email = user.Email;
            
            return View(passwordChangeViewModel);
        }
        
        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            var user = _dbcontext.Users.FirstOrDefault(u => u.Email == passwordChangeViewModel.Email);
            ViewData["Post"] = true;
            if (user != null)
            {
                user.Password = BC.HashPassword(passwordChangeViewModel.Password);
                _dbcontext.SaveChanges();
                
                ViewData["Text"] = "Пароль успешно сменен. Пожалуйста, войдите в учетную запись с новым паролем.";
            }
            else
            {
                ViewData["Text"] = "Ошибка. Пользователь не найден";
            }
            return View();
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
                    if (!user.EmailConfirmed)
                    {
                        ModelState.AddModelError("", "Ваша учетная запись еще не была активирована.");
                        return View();
                    }
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
        
        public IActionResult UserCreated()
        {
            return View();
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult Manage()
        {
            User user = _dbcontext.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

            if (user != null)
            {
                var model = new UserViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Tel = user.Tel
                };
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult Manage(UserViewModel model)
        {
            User user = _dbcontext.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

            if (user != null)
            {
                if (model.FirstName != user.FirstName)
                {
                    user.FirstName = model.FirstName;
                }
                if (model.LastName != user.LastName)
                {
                    user.LastName = model.LastName;
                }
                if (model.Tel != user.Tel)
                {
                    user.Tel = model.Tel;
                }
            }

            return View(model);
        }
        
        [Authorize]
        public IActionResult Orders()
        {
            return View();
        }
        
        public IActionResult Works()
        {
            return View();
        }
        
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
    }
}
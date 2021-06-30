using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using designer_website.Filters;
using designer_website.Interfaces;
using designer_website.Models;
using designer_website.Models.EntityFrameworkModels;
using designer_website.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
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

                return RedirectToAction("SendConfirmationLetter", user);
            }   
            
            return View(registerViewModel); // Если валидация не прошла, возвращаемся на страницу регистрации.
        }
        
        [AnonymousOnlyFilter]
        public IActionResult SendConfirmationLetter(User user)
        {
            User sameUser = _dbcontext.Users.FirstOrDefault(u => u.Email == user.Email);
            
            int responceId;
            
            if (sameUser == null)
            {
                string token = _tokenizer.GetRandomToken();
                string url = "https://" + HttpContext.Request.Host + "/Account/EmailConfirmation/" + token;
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
                        responceId = 1;
                        //text = "Аккаунт дизайнера был создан, письмо с подтверждением регистрации было " + 
                                           //"отправлено на почту дизайнера.";
                    }
                    else
                    {
                        responceId = 2;
                        //text = "Ваш аккаунт был создан, чтобы подтвердить регистрацию и активировать аккаунт, " +
                                           //"пройдите по ссылке из письма, которое мы отправили Вам на почту.";
                    }
                    
                    _dbcontext.Users.Add(user);
                    _dbcontext.SaveChanges();
                }
                else
                {
                    responceId = 3;
                    //text = "Ошибка при отправке письма, проверьте введенную Вами при регистрации почту " + 
                                       //"и попробуйте пройти регистрацию еще раз.";
                }
            }
            else
            {
                responceId = 4;
                //text = "Ошибка при отправке письма.";
            }
            
            return RedirectToAction("EmailConfirmation", "Account", new { id = responceId});
        }
        
        [Route("Account/EmailConfirmation/{responseId:int}")]
        public IActionResult EmailConfirmation(int responseId)
        {   
            switch (responseId)
            {
                case 1:
                    ViewData["Text"] = "Аккаунт дизайнера был создан, письмо с подтверждением регистрации было " + 
                                        "отправлено на почту дизайнера.";
                    break;
                case 2:
                    ViewData["Text"] = "Ваш аккаунт был создан, чтобы подтвердить регистрацию и активировать аккаунт, " +
                                        "пройдите по ссылке из письма, которое мы отправили Вам на почту.";
                    break;
                case 3:
                    ViewData["Text"] = "Ошибка при отправке письма, проверьте введенную Вами при регистрации почту " + 
                                        "и попробуйте пройти регистрацию еще раз.";
                    break;
                default:
                    ViewData["Text"] = "Ошибка при отправке письма.";
                    break;
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
        public IActionResult Recovery()
        {
            ViewData["Post"] = false;
            return View();
        }

        [HttpPost]
        public IActionResult Recovery(UserViewModel recoveryViewModel)
        {
            ViewData["Post"] = true;

            string userEmail;

            if (User.Identity.IsAuthenticated)
            {
                userEmail = User.Identity.Name;
            }
            else
            {
                userEmail = recoveryViewModel.Email;
            }
            
            User sameUser = _dbcontext.Users.FirstOrDefault(u => u.Email == userEmail);
            if (sameUser != null)
            {
                string token = _tokenizer.GetRandomToken();
                string url = "https://" + HttpContext.Request.Host + "/Account/PasswordChange/" + token;

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
        public IActionResult PasswordChange(string token)
        {
            var user = _dbcontext.Users.FirstOrDefault(u => u.Token == token);
            
            if (user == null)
            {
                ViewData["Post"] = true;
                ViewData["Text"] = "Ошибка. Неверная ссылка.";
                return View();
            }
            else
            {
                ViewData["Post"] = false;
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
                user.Token = null;
                _dbcontext.SaveChanges();

                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Logout");
                }

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
        public IActionResult Profile()
        {
            User user;
            if (User.IsInRole("Designer"))
            {
                user = _dbcontext.Users
                    .Include(u => u.Role)
                    .Include(u => u.DesignerOrderInfoIds)
                        .ThenInclude(d => d.Order)
                            .ThenInclude(o => o.Service)
                    .Include(u => u.DesignerOrderInfoIds)
                        .ThenInclude(d => d.Order)
                            .ThenInclude(o => o.DesignerOrderInfoIds)
                                .ThenInclude(d => d.User)
                    .Include(u => u.UserWorks)
                        .ThenInclude(w => w.Work)
                            .ThenInclude(w => w.Service)
                    .Include(u => u.UserWorks)
                        .ThenInclude(w => w.Work)
                           .ThenInclude(w => w.UserWorks)
                               .ThenInclude(w => w.User)
                    .FirstOrDefault(u => u.Email == User.Identity.Name);
            }
            else
            {
                user = _dbcontext.Users
                    .Include(u => u.Role)
                    .Include(u => u.OrderInfos)
                        .ThenInclude(o => o.Service)
                    .Include(u => u.OrderInfos)
                        .ThenInclude(o => o.DesignerOrderInfoIds)
                            .ThenInclude(d => d.User)
                    .Include(u => u.UserWorks)
                        .ThenInclude(w => w.Work)
                            .ThenInclude(w => w.Service)
                    .Include(u => u.UserWorks)
                        .ThenInclude(w => w.Work)
                            .ThenInclude(w => w.UserWorks)
                                .ThenInclude(w => w.User)
                    .FirstOrDefault(u => u.Email == User.Identity.Name);
            }

            
            if (user != null)
            {
                var model = ProfileViewModel.FillProfileViewModel(user, _dbcontext);
                
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult Profile(ProfileViewModel model)
        {
            User user = _dbcontext.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

            if (user != null)
            {
                if (model.User.FirstName != user.FirstName)
                {
                    user.FirstName = model.User.FirstName;
                }
                if (model.User.LastName != user.LastName)
                {
                    user.LastName = model.User.LastName;
                }
                if (model.User.Tel != user.Tel)
                {
                    user.Tel = model.User.Tel;
                }

                _dbcontext.SaveChanges();
            }

            return View(model);
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult NewOrder(NewOrderInitialModel initialModel)
        {
            var model = new OrderViewModel();

            if (initialModel.ServiceId != null)
            {
                model.ChosenService = _dbcontext.Services.FirstOrDefault(s => s.ServiceId == initialModel.ServiceId);
            }

            if (initialModel.FirstDesigner != null)
            {
                var user = _dbcontext.Users.FirstOrDefault(u => u.UserId == initialModel.FirstDesigner);

                if (user != null)
                {
                    model.FirstDesigner = UserViewModel.ToUserViewModel(user);
                }
            }

            if (initialModel.SecondDesigner != null)
            {
                var user = _dbcontext.Users.FirstOrDefault(u => u.UserId == initialModel.SecondDesigner);

                if (user != null)
                {
                    model.SecondDesigner = UserViewModel.ToUserViewModel(user);
                }
            }

            var designers = _dbcontext.Users.Where(
                u => u.Role == _dbcontext.Roles.FirstOrDefault(r => r.RoleName == "Designer")).ToList();
            
            var designersModels = new List<UserViewModel>();
            
            foreach (var designer in designers)
            {
                designersModels.Add(UserViewModel.ToUserViewModel(designer));
            }

            model.AllDesigners = designersModels;

            return View(model);
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult NewOrder(OrderViewModel model)
        {
            OrderInfo order = new OrderInfo();

            var currentUser = _dbcontext.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

            if (currentUser == null)
            {
                return RedirectToAction("Logout");
            }

            order.User = currentUser;
            order.Date = DateTime.Now;

            var chosenService = _dbcontext.Services.FirstOrDefault(s => s.ServiceId == model.ChosenServiceId);

            if (chosenService == null)
            {
                ModelState.AddModelError("", "Услуга недоступна");
                return View(model);
            }

            order.Service = chosenService;
            order.OrderDescription = model.Description;
            order.Price = chosenService.DefaultPrice;

            var designers = new List<DesignerOrderInfoId>();

            if (model.FirstDesignerId != null)
            {
                var user = _dbcontext.Users.FirstOrDefault(u => u.UserId == model.FirstDesignerId);

                if (user == null)
                {
                    ModelState.AddModelError("", "Ошибка выбора дизайнера 1");
                    return View(model);
                }

                var entry = new DesignerOrderInfoId();
                entry.User = user;
                entry.Order = order;
                
                order.DesignerOrderInfoIds.Add(entry);
                designers.Add(entry);
            }
            
            if (model.SecondDesignerId != null && model.FirstDesignerId != model.SecondDesignerId)
            {
                var user = _dbcontext.Users.FirstOrDefault(u => u.UserId == model.SecondDesignerId);

                if (user == null)
                {
                    ModelState.AddModelError("", "Ошибка выбора дизайнера 2");
                    return View(model);
                }

                var entry = new DesignerOrderInfoId();
                entry.User = user;
                entry.Order = order;
                
                order.DesignerOrderInfoIds.Add(entry);
                designers.Add(entry);
            }

            _dbcontext.OrderInfos.Add(order);
            _dbcontext.DesignerOrderInfoIds.AddRange(designers);
            _dbcontext.SaveChanges();

            return RedirectToAction("OrderEmailSending", new { orderId = order.OrderId });
        }
        
        [Authorize]
        [Route("Account/OrderEmailSending/{orderId:int}")]
        public IActionResult OrderEmailSending(int orderId)
        {
            var user = _dbcontext.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            var order = _dbcontext.OrderInfos
                .Include(o => o.Service).FirstOrDefault(o => o.OrderId == orderId);
            
            if (user != null && order != null)
            {
                var to = new MailboxAddress(user.FirstName, user.Email);
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<p>Вы успешно создали заказ " + orderId + "</p><br/>" +
                                       "<p>Услуга: " + order.Service.ServiceName + "</p><br/>" +
                                       "<p>Стоимость: " + order.Price + "</p><br/>" +
                                       "<p>Дата: " + order.Date.Date + "</p><br/>" +
                                       "<p>Описание: " + order.OrderDescription + "</p>";
                
                bodyBuilder.TextBody = "Вы успешно создали заказ " + orderId + "\n" +
                                       "Услуга: " + order.Service.ServiceName + "\n" +
                                       "Стоимость: " + order.Price + "$\n" +
                                       "Дата: " + order.Date.Date + "\n" +
                                       "Описание: " + order.OrderDescription;
        
                var sendEmail = _emailSender
                    .TryToSendMail(to, "Заказ успешно создан", bodyBuilder.ToMessageBody());

                if (sendEmail == EmailResult.SendSuccess)
                {
                    ViewData["Text"] = "Вы успешно создали заказ. Письмо с деталями Вашего заказа было отправлено" +
                                       "на Вашу почту";
                }
                else
                {
                    ViewData["Text"] = "Вы успешно создали заказ. Произошла ошибка при попытке отправить письмо " +
                                       "с деталями Вашего заказа на Вашу почту";
                }

                return View();
            }

            return RedirectToAction("Index", "Home");
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
                Email = "designer4@gg",
                FirstName = "designer",
                LastName = "fourth",
                Password = BC.HashPassword("designer12345"),
                Role = _dbcontext.Roles.FirstOrDefault(r => r.RoleName == "Designer"),
                Tel = "2",
                EmailConfirmed = true
            };
            
            _dbcontext.Users.Add(designer);
            _dbcontext.SaveChanges();
            
            return RedirectToAction("Index", "Home");
        }
        */
    }
}
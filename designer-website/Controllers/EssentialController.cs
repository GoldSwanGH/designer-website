using System.Linq;
using designer_website.Models;
using designer_website.Models.EntityFrameworkModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace designer_website.Controllers
{
    public class EssentialController : Controller
    {
        private readonly MSDBcontext _dbcontext;
        public EssentialController(MSDBcontext dbcontext)
        {
           _dbcontext = dbcontext;
        }
        public IActionResult Authors()
        {
            var authors = _dbcontext.Users.Where(u =>
                u.Role == _dbcontext.Roles.FirstOrDefault(r => r.RoleName == "Designer")).ToList();
            
            var model = new UserCollectionViewModel();
            foreach (var user in authors)
            {
                model.Users.Add(new UserViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Tel = user.Tel
                });
            }
            
            return View(model);
        }

        public IActionResult Author(UserViewModel model)
        {
            return View(model);
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult CreateOrder()
        {
            return View();
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult CreateOrder(OrderViewModel model)
        {
            return View();
        }
    }
}
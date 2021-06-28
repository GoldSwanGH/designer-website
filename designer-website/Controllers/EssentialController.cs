using System;
using System.Collections.Generic;
using System.Linq;
using designer_website.Models;
using designer_website.Models.EntityFrameworkModels;
using designer_website.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var authors = _dbcontext.Users
                .Where(u => u.Role == _dbcontext.Roles.FirstOrDefault(r => r.RoleName == "Designer")).ToList();
            
            var model = new UserCollectionViewModel();

            foreach (var author in authors)
            {
                model.UserList.Add(UserViewModel.ToUserViewModel(author));
            }
            
            return View(model);
        }

        public IActionResult Author(UserViewModel model)
        {
            var profile = new ProfileViewModel();

            var user = _dbcontext.Users.FirstOrDefault(u => u.UserId == model.userId);

            if (user != null)
            {
                profile = ProfileViewModel.FillProfileViewModel(user, _dbcontext);
            }
            else
            {
                return RedirectToAction("Market", "Essential");
            }

            return View(model);
        }

        public IActionResult Market()
        {
            var model = new ServiceCollectionViewModel();
            model.Services = _dbcontext.Services.ToList();

            return View(model);
        }
    }
}
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
            var authors = _dbcontext.Users.Where(u =>
                u.Role == _dbcontext.Roles.FirstOrDefault(r => r.RoleName == "Designer")).ToList();
            
            var model = new ProfileCollectionViewModel();

            foreach (var author in authors)
            {
                model.ProfileList.Add(ProfileViewModel.FillProfileViewModel(author, _dbcontext));
            }
            
            return View(model);
        }

        public IActionResult Author(ProfileViewModel model)
        {
            return View(model);
        }

        public IActionResult Market()
        {
            return View();
        }
    }
}
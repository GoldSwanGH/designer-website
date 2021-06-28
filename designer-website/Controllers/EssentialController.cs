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
                .Where(u => u.Role == _dbcontext.Roles.FirstOrDefault(r => r.RoleName == "Designer")).ToList();
            
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
            var model = new ServiceCollectionViewModel();
            model.Services = _dbcontext.Services.ToList();

            return View(model);
        }
    }
}
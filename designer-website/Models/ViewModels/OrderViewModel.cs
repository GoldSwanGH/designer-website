using System;
using System.Collections.Generic;
using designer_website.Models.EntityFrameworkModels;
using Microsoft.Extensions.DependencyInjection;

namespace designer_website.Models
{
    public class OrderViewModel
    {
        public List<Service> Services { get; set; }
        
        public Service ChosenService { get; set; }
        
        public List<UserViewModel> Designers { get; set; }
        
        public string Description { get; set; }
    }
}
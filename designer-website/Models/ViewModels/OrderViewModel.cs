using System;
using System.Collections.Generic;
using designer_website.Models.EntityFrameworkModels;
using Microsoft.Extensions.DependencyInjection;

namespace designer_website.Models
{
    public class OrderViewModel
    {

        public Service ChosenService { get; set; }
        
        public int? ChosenServiceId { get; set; }
        
        public UserViewModel[] ChosenDesigners { get; set; }
        
        public List<UserViewModel> AllDesigners { get; set; }

        public string Description { get; set; }
        
        public int? Price { get; set; }

        public OrderViewModel()
        {
            ChosenService = new Service();
            ChosenDesigners = new UserViewModel[2];
            AllDesigners = new List<UserViewModel>();
        }
    }
}
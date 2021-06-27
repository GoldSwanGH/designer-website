using System.Collections.Generic;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Models.ViewModels
{
    public class OrderDesignersViewModel
    {
        public OrderInfo Order { get; set; }
        public List<UserViewModel> Designers { get; set; }
    }
}
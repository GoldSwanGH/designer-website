using System.Collections.Generic;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Models.ViewModels
{
    public class UserWorksViewModel
    {
        public Work Work { get; set; }
        public List<UserViewModel> Designers { get; set; }
    }
}
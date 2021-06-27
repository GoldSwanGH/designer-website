using System.Collections.Generic;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Models.ViewModels
{
    public class ProfileViewModel
    {
        public UserViewModel User { get; set; }
        
        public List<OrderDesignersViewModel> OrderInfos { get; set; }
        
        public List<UserWorksViewModel> Works { get; set; }
    }
}
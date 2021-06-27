using System.Collections.Generic;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Models.ViewModels
{
    public class UserCollectionViewModel
    {
        public List<ProfileViewModel> Users { get; set; }
        
        public UserCollectionViewModel()
        {
            Users = new List<ProfileViewModel>();
        }
    }
}
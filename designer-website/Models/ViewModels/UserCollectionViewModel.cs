using System.Collections.Generic;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Models
{
    public class UserCollectionViewModel
    {
        public List<UserViewModel> Users { get; set; }
        
        public UserCollectionViewModel()
        {
            Users = new List<UserViewModel>();
        }
    }
}
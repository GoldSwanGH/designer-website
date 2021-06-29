using System.Collections.Generic;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Models.ViewModels
{
    public class UserCollectionViewModel
    {
        public List<UserViewModel> UserList { get; set; }

        public UserCollectionViewModel()
        {
            UserList = new List<UserViewModel>();
        }
    }
}
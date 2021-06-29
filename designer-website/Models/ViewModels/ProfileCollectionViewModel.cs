using System.Collections.Generic;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Models.ViewModels
{
    public class ProfileCollectionViewModel
    {
        public List<ProfileViewModel> ProfileList { get; set; }
        
        public ProfileCollectionViewModel()
        {
            ProfileList = new List<ProfileViewModel>();
        }
    }
}
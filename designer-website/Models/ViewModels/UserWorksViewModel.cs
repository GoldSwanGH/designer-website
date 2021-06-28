using System.Collections.Generic;
using System.Linq;
using designer_website.Models.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;

namespace designer_website.Models.ViewModels
{
    public class UserWorksViewModel
    {
        public Work Work { get; set; }
        public List<UserViewModel> Designers { get; set; }

        public UserWorksViewModel()
        {
            Work = new Work();
            Designers = new List<UserViewModel>();
        }

        public static UserWorksViewModel FillUserWorksViewModel(UserWork userWork, MSDBcontext dbContext)
        {
            var userWorks = new UserWorksViewModel
            {
                Work = userWork.Work
            };

            var userWorksUserWorks = dbContext.UserWorks
                .Include(uw => uw.User)
                .Where(w => w.Work == userWork.Work).ToList();

            if (userWorksUserWorks.Any())
            {
                foreach (var entry in userWorksUserWorks)
                {
                    var designer = UserViewModel.ToUserViewModel(entry.User);
                    userWorks.Designers.Add(designer);
                }
            }


            return userWorks;
        }
    }
}
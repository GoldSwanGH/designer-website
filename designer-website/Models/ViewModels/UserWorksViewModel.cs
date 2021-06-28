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

        public static UserWorksViewModel FillUserWorksViewModel(UserWork userWork, MSDBcontext dbContext)
        {
            var userWorks = new UserWorksViewModel
            {
                Work = userWork.Work
            };

            var userWorksUserWorks = dbContext.UserWorks.Include(uw => uw.User).Where(w => w.Work == userWork.Work);

            if (userWorksUserWorks.Count() >= 1)
            {
                foreach (var entry in userWorksUserWorks)
                {
                    userWorks.Designers.Add(UserViewModel.ToUserViewModel(entry.User));
                }
            }


            return userWorks;
        }
    }
}
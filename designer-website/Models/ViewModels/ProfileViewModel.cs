using System.Collections.Generic;
using System.Linq;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Models.ViewModels
{
    public class ProfileViewModel
    {
        public UserViewModel User { get; set; }
        
        public List<OrderDesignersViewModel> OrderDesignersList { get; set; }
        
        public List<UserWorksViewModel> UserWorksList { get; set; }

        public ProfileViewModel()
        {
            OrderDesignersList = new List<OrderDesignersViewModel>();
            UserWorksList = new List<UserWorksViewModel>();
        }

        public static ProfileViewModel FillProfileViewModel(User user, MSDBcontext dbContext)
        {
            var model = new ProfileViewModel();

            model.User = UserViewModel.ToUserViewModel(user);
            model.OrderDesignersList = new List<OrderDesignersViewModel>();
            model.UserWorksList = new List<UserWorksViewModel>();

            if (user.Role.RoleName == "Designer")
            {
                if (user.DesignerOrderInfoIds != null && user.DesignerOrderInfoIds.Any())
                {
                    foreach (var entry in user.DesignerOrderInfoIds)
                    {
                        model.OrderDesignersList
                            .Add(OrderDesignersViewModel.FillOrderDesignersViewModel(entry.Order, dbContext));
                    }
                }
            }
            else
            {
                if (user.OrderInfos != null && user.OrderInfos.Count != 0)
                {
                    foreach (var orderInfo in user.OrderInfos)
                    {
                        model.OrderDesignersList
                            .Add(OrderDesignersViewModel.FillOrderDesignersViewModel(orderInfo, dbContext));
                    }
                }
            }

            if (user.UserWorks != null && user.UserWorks.Count != 0)
            {
                foreach (var userWork in user.UserWorks)
                {
                    model.UserWorksList
                        .Add(UserWorksViewModel.FillUserWorksViewModel(userWork, dbContext));
                }
            }
            
            return model;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using designer_website.Models.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;

namespace designer_website.Models.ViewModels
{
    public class OrderDesignersViewModel
    {
        public OrderInfo Order { get; set; }
        public List<UserViewModel> Designers { get; set; }

        public OrderDesignersViewModel()
        {
            Order = new OrderInfo();
            Designers = new List<UserViewModel>();
        }

        public static OrderDesignersViewModel FillOrderDesignersViewModel(OrderInfo order, MSDBcontext dbContext)
        {
            var orderDesigners = new OrderDesignersViewModel
            {
                Order = order
            };

            var orderDesignersEntries = 
                dbContext.DesignerOrderInfoIds.Where(d => d.Order == order)
                    .Include(d => d.User);

            if (orderDesignersEntries.Any())
            {
                foreach (var entry in orderDesignersEntries)
                {
                    orderDesigners.Designers.Add(UserViewModel.ToUserViewModel(entry.User));
                }
            }

            return orderDesigners;
        }
    }
}
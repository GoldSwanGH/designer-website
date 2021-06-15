using System;
using System.Collections.Generic;

#nullable disable

namespace designer_website
{
    public partial class User
    {
        public User()
        {
            DesignerOrderInfoIds = new HashSet<DesignerOrderInfoId>();
            OrderInfos = new HashSet<OrderInfo>();
            UserWorks = new HashSet<UserWork>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? Tel { get; set; }
        public int RoleId { get; set; }
        public int OptionsId { get; set; }

        public virtual UserOption Options { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<DesignerOrderInfoId> DesignerOrderInfoIds { get; set; }
        public virtual ICollection<OrderInfo> OrderInfos { get; set; }
        public virtual ICollection<UserWork> UserWorks { get; set; }
    }
}

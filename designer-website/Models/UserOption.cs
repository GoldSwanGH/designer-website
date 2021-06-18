using System;
using System.Collections.Generic;

#nullable disable

namespace designer_website.Models
{
    public partial class UserOption
    {
        public UserOption()
        {
            Users = new HashSet<User>();
        }

        public int OptionsId { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace designer_website.Models.EntityFrameworkModels
{
    public partial class UserWork
    {
        public int UserWorkId { get; set; }
        public int UserId { get; set; }
        public int WorkId { get; set; }

        public virtual User User { get; set; }
        public virtual Work Work { get; set; }
    }
}

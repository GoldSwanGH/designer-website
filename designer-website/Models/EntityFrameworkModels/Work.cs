using System;
using System.Collections.Generic;

#nullable disable

namespace designer_website.Models.EntityFrameworkModels
{
    public partial class Work
    {
        public Work()
        {
            UserWorks = new HashSet<UserWork>();
        }

        public int WorkId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string WorkName { get; set; }
        public int ServiceId { get; set; }

        public virtual Service Service { get; set; }
        public virtual ICollection<UserWork> UserWorks { get; set; }
    }
}

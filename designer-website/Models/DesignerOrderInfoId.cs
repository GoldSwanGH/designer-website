using System;
using System.Collections.Generic;

#nullable disable

namespace designer_website.Models
{
    public partial class DesignerOrderInfoId
    {
        public int DesignerOrderInfoId1 { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }

        public virtual OrderInfo Order { get; set; }
        public virtual User User { get; set; }
    }
}

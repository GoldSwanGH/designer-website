using System;
using System.Collections.Generic;

#nullable disable

namespace designer_website
{
    public partial class OrderInfo
    {
        public OrderInfo()
        {
            DesignerOrderInfoIds = new HashSet<DesignerOrderInfoId>();
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int? ServiceId { get; set; }
        public string OrderDescription { get; set; }

        public virtual Service Service { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<DesignerOrderInfoId> DesignerOrderInfoIds { get; set; }
    }
}

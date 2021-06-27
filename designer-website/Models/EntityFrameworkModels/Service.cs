﻿using System;
using System.Collections.Generic;

#nullable disable

namespace designer_website.Models.EntityFrameworkModels
{
    public partial class Service
    {
        public Service()
        {
            OrderInfos = new HashSet<OrderInfo>();
        }

        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int? DefaultPrice { get; set; }
        public string ServiceDescription { get; set; }

        public virtual ICollection<OrderInfo> OrderInfos { get; set; }
    }
}

using System.Collections.Generic;

namespace designer_website.Models.ViewModels
{
    public class NewOrderInitialModel
    {
        public int? ServiceId { get; set; }
        
        public List<string> Designers { get; set; }
    }
}
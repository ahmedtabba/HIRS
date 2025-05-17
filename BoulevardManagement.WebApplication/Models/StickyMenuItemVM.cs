using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoulevardManagement.WebApplication.Models
{
    public class StickyMenuItemVM
    {
        public string Text { get; set; }
        public string Id { get; set; }
        public bool IsActive { get; set; }
    }
}
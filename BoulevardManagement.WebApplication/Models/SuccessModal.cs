using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoulevardManagement.WebApplication.Models
{
    public class SuccessModal : ConfirmModel
    {
        public bool ShowCancelBtn { get; set; }
        public string OkURL { get; set; }
    }
}
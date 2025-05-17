using BoulevardManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoulevardManagement.WebApplication.Models
{
    public class MonitoringTheVitalSignsVM
    {
        public int TeleMentalHealthId { get; set; }
        public CaseStatus CaseStatus { get; set; }
    }
}
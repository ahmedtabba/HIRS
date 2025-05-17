using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoulevardManagement.WebApplication.Models.Home
{
    public class DashboardVM
    {
        public List<CaseInMonth> TeleMHCases { get; set; }
        public List<CaseInMonth> NPICUCases { get; set; }
        public List<CaseInMonth> ICUCases { get; set; }

        public int PatientsInTeleMH { get; set; }
        public int PatientsInNPICU { get; set; }
        public int PatientsInICU { get; set; }

        public bool ShowMH { get; set; }
        public bool ShowNPICU { get; set; }
        public bool ShowICU { get; set; }

        public int MHSPUsersCount { get; set; }
        public int MHConUsersCount { get; set; }
        public int NPICUSPUsersCount { get; set; }
        public int NPICUConUsersCount { get; set; }
        public int ICUSPUsersCount { get; set; }
        public int ICUConUsersCount { get; set; }
        public int ITUsersCount { get; set; }

        public DashboardVM()
        {
            TeleMHCases = new List<CaseInMonth>();
            NPICUCases = new List<CaseInMonth>();
            ICUCases = new List<CaseInMonth>();
        }
    }

    public class CaseInMonth
    {
        public int CaseCount { get; set; }
        public string MonthName { get; set; }
    }
}
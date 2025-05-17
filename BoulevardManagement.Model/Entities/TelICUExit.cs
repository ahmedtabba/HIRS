using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelICUExit : Entity
    {
        public int TeleICUId { get; set; }
        public TeleICU TeleICU { get; set; }
        public string BloodPressure { get; set; }
        public string Diagnosis { get; set; }
        public decimal? Temperature { get; set; }
        public string Temperature_Recovery { get; set; }
        public int? Oxygenation { get; set; }
        public int? Pulse { get; set; }
        public string Recommendations { get; set; }
        public string Medication { get; set; }

    }
}

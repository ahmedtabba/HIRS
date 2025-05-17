using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelICUVitalSign : Entity
    {
        public int TeleICUId { get; set; }
        public TeleICU TeleICU { get; set; }
        public DateTime VitalSignDate { get; set; }
        public string Heat { get; set; }
        public string Pressure { get; set; }
        public string Pulse { get; set; }
        public string Oxygenation { get; set; }
        public string BloodGlucoseLevels{ get; set; }
        public string EmptyingOfUrine{ get; set; }
        public string Drain { get; set; }
        public string SecondDrain { get; set; }
        public string Notes { get; set; }

    }
}

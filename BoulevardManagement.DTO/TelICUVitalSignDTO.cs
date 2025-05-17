using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.DTO
{
    public class TelICUVitalSignDTO : EntityDTO
    {
        public int TeleICUId { get; set; }
        public string EncrptedTeleICUId { get { return HashIdsManager.Encrypt(TeleICUId); } set { } }
        public DateTime VitalSignDate { get; set; }
        public string Heat { get; set; }
        public string Pressure { get; set; }
        public string Pulse { get; set; }
        public string Oxygenation { get; set; }
        public string BloodGlucoseLevels { get; set; }
        public string EmptyingOfUrine { get; set; }
        public string Drain { get; set; }
        public string SecondDrain { get; set; }
        public string Notes { get; set; }
        public TelICUVitalSignDTO()
        {
            VitalSignDate = DateTime.Now;
        }
    }
}

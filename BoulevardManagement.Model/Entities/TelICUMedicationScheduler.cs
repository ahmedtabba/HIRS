using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelICUMedicationScheduler : Entity
    {
        public int TeleICUId { get; set; }
        public TeleICU TeleICU { get; set; }
        public string MedicationName { get; set; }
        public string Concentration{ get; set; }
        public int NumberOfTimes { get; set; }
        public string RouteOfAdministratinOfDrug{ get; set; }
        public bool Morning_8  { get; set; }
        public bool Morning_10 { get; set; }
        public bool Afternoon_12 { get; set; }
        public bool Afternoon_2 { get; set; }
        public bool Afternoon_4 { get; set; }
        public bool Night_6 { get; set; }
        public bool Night_8 { get; set; }
        public bool Night_10 { get; set; }
        public bool Morning_12 { get; set; }
        public bool Morning_2 { get; set; }
        public bool Morning_4 { get; set; }
        public bool Morning_6 { get; set; }


    }
}

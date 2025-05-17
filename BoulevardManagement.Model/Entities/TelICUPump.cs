using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelICUPump : Entity
    {

        public int TeleICUId { get; set; }
        public TeleICU TeleICU { get; set; }
        public DateTime PumpDate { get; set; }
        public string MedicationName { get; set; }
        public string FlowRate { get; set; }
        public string Notes { get; set; }

    }
}

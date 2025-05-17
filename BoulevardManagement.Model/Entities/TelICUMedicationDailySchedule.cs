using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelICUMedicationDailySchedule : Entity
    {
        public int TeleICUId { get; set; }
        public TeleICU TeleICU { get; set; }
        public string Epidemiology { get; set; }
        public string ChronicDiseases { get; set; }

    }
}

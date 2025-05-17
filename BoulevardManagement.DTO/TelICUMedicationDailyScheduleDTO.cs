using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BoulevardManagement.DTO
{
    public class TelICUMedicationDailyScheduleDTO : EntityDTO
    {
        public int TeleICUId { get; set; }
        public string Epidemiology { get; set; }
        public string ChronicDiseases { get; set; }
        public CaseStatus CaseStatus { get; set; }
    }
}

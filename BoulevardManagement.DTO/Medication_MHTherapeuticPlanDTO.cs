using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class Medication_MHTherapeuticPlanDTO : EntityDTO
    {
        public int MedicationId { get; set; }
        public string MedicationName { get; set; }
        public int TelMHTherapeuticPlanId { get; set; }
        public string Descreption { get; set; }
    }
}

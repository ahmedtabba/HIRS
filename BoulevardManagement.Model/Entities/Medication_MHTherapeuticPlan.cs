using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class Medication_MHTherapeuticPlan : Entity
    {
        public int MedicationId { get; set; }
        public Medication Medication { get; set; }
        public int TelMHTherapeuticPlanId { get; set; }
        public TelMHTherapeuticPlan TelMHTherapeuticPlan { get; set; }
        public string Descreption { get; set; }

    }
}

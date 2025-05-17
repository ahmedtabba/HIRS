using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelMHTherapeuticPlan:Entity
    {
        public int TeleMentalHealthId { get; set; }
        public TeleMentalHealth TeleMentalHealth { get; set; }

        public string TherapeuticPlan { get; set; }
        public string TherapeuticPlanVoiceAttachPath { get; set; }
        public ICollection<Medication_MHTherapeuticPlan> Medications { get; set; }
        public int? NotesAttachmentId { get; set; }
     
        public string Notes { get; set; }
        public TelMHTherapeuticPlan()
        {
            Medications = new List<Medication_MHTherapeuticPlan>();
        }

    }
}

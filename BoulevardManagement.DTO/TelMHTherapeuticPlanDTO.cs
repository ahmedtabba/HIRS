using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class TelMHTherapeuticPlanDTO:EntityDTO
    {
        public int TeleMentalHealthId { get; set; }

        public string TherapeuticPlan { get; set; }
        public string TherapeuticPlanVoiceAttachPath { get; set; }
        public bool IsTherapeuticPlanVoiceDeleted { get; set; }

        public DateTime DateOfCreate { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }


        public int? NotesAttachmentId { get; set; }
        public bool HasNotesAttachment { get; set; }
        public string NotesFilePath { get; set; }
        public string NotesFilename { get; set; }
        public string Notes { get; set; }
        public CaseStatus CaseStatus { get; set; }
        public ICollection<Medication_MHTherapeuticPlanDTO> Medications { get; set; }
        public TelMHTherapeuticPlanDTO()
        {
            Medications = new List<Medication_MHTherapeuticPlanDTO>();
        }

    }
}

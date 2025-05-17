using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class NPICUConsultantSectionDTO:EntityDTO
    {
        public int NPICUId { get; set; }
        public string ConsultationNote { get; set; }
        public string ConsultationNoteVoiceAttachPath { get; set; }
        public bool IsConsultationNoteVoiceDeleted { get; set; }
        public DateTime DateOfCreate { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }

        public int? ConsultationNoteAttachmentId { get; set; }
        public bool HasConsultationNoteAttachment { get; set; }
        public string ConsultationNoteFilePath { get; set; }
        public string ConsultationNoteFilename { get; set; }

        public CaseStatus CaseStatus { get; set; }
    }
}

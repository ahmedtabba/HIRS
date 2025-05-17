using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BoulevardManagement.DTO
{
    public class TelICUConsultationFormDTO : EntityDTO
    {
        public int TeleICUId { get; set; }
        public string Notes { get; set; }
        public int? NotesAttachmentId { get; set; }
        public bool HasNotesAttachment { get; set; }
        public string NotesFilePath { get; set; }
        public string NotesFilename { get; set; }
        public string NotesVoiceAttachPath { get; set; }
        public bool IsNotesVoiceDeleted { get; set; }

        public CaseStatus CaseStatus { get; set; }
        public DateTime DateOfCreate { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
    }
}

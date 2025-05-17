using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class NPICUConsultantSection:Entity
    {
        public int NPICUId { get; set; }
        public NPICU NPICU { get; set; }
        public string ConsultationNote { get; set; }
        public string ConsultationNoteVoiceAttachPath { get; set; }
        public int? ConsultationNoteAttachmentId { get; set; }
    }
}

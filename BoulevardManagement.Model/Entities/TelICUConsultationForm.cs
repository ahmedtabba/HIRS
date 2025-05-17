using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelICUConsultationForm : Entity
    {
        public int TeleICUId { get; set; }
        public TeleICU TeleICU { get; set; }
        public string Notes { get; set; }
        public int? NotesAttachmentId { get; set; }
        public string NotesVoiceAttachPath { get; set; }


    }
}

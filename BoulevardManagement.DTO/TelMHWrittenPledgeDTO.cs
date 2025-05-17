using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class TelMHWrittenPledgeDTO : EntityDTO
    {
        public int TeleMentalHealthId { get; set; }

        public int? PledgeDocumentAttachmentId { get; set; }
        public bool HasNewPledgeDocumentAttachment { get; set; }
        public string PledgeDocumentFilePath { get; set; }
        public string PledgeDocumentFilename { get; set; }
        public int? PatientIDCardAttachmentId { get; set; }
        public bool HasNewPatientIDCardAttachment { get; set; }
        public string PatientIDCardFilePath { get; set; }
        public string PatientIDCardFilename { get; set; }
        public CaseStatus CaseStatus { get; set; }
    }
}

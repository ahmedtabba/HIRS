using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class TelMHPhysicalExaminationReportDTO : EntityDTO
    {
        public int TeleMentalHealthId { get; set; }

        public string ReportOfThePhysicalExaminationCommittee { get; set; }

        public int? ReportOfThePhysicalExaminationAttachmentId { get; set; }
        public bool HasNewReportOfThePhysicalExaminationAttachment { get; set; }
        public string ReportOfThePhysicalExaminationFilePath { get; set; }
        public string ReportOfThePhysicalExaminationFilename { get; set; }

        public string PatientName { get; set; }
        public CaseStatus CaseStatus { get; set; }

    }
}

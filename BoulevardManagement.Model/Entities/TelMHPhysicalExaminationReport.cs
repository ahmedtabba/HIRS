using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelMHPhysicalExaminationReport : Entity
    {
        public int TeleMentalHealthId { get; set; }
        public TeleMentalHealth TeleMentalHealth { get; set; }

        public string ReportOfThePhysicalExaminationCommittee { get; set; }
        public int? ReportOfThePhysicalExaminationAttachmentId { get; set; }

    }
}

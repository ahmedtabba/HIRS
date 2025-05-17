using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelMHWrittenPledge : Entity
    {
        public int TeleMentalHealthId { get; set; }
        public TeleMentalHealth TeleMentalHealth { get; set; }

        public int? PledgeDocumentAttachmentId { get; set; }
        public int? PatientIDCardAttachmentId { get; set; }
    }
}

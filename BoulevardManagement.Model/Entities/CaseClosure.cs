using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class CaseClosure:Entity
    {
        public int CaseId { get; set; }
        public int CaseDepartment { get; set; }
        public int CaseCloseReason { get; set; }
        public string Notes { get; set; }
        public int? CaseClosureAttachmentId { get; set; }
    }
}

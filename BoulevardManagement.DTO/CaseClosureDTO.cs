using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class CaseClosureDTO : EntityDTO
    {
        public int CaseId { get; set; }
        public DepartmentEnum CaseDepartment { get; set; }
        public CaseCloseReason CaseCloseReason { get; set; }
        public string Notes { get; set; }
        public int? CaseClosureAttachmentId { get; set; }
        public bool HasCaseClosureAttachment { get; set; }
        public string CaseClosureFilename { get; set; }
        public string CaseClosureFilePath { get; set; }
        public bool CanReOpenCase { get; set; }
        public DateTime DateOfCreate { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string CloseUrl
        {
            get
            {

                switch (CaseDepartment)
                {
                    case DepartmentEnum.MH:
                        return "/TeleMentalHealth/CloseCase";
                    case DepartmentEnum.NPICU:
                        return "/NPICU/CloseCase";
                    case DepartmentEnum.ICU:
                        return "/TeleICU/CloseCase";
                    default:
                        return "#";
                }

            }
        }

        public string ReOpenUrl
        {
            get
            {

                switch (CaseDepartment)
                {
                    case DepartmentEnum.MH:
                        return "/TeleMentalHealth/ReOpenCase";
                    case DepartmentEnum.NPICU:
                        return "/NPICU/ReOpenCase";
                    case DepartmentEnum.ICU:
                        return "/TeleICU/ReOpenCase";
                    default:
                        return "#";
                }

            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class TelMHDiagnosisDTO:EntityDTO
    {
        public int TelMHClinicalStoryId { get; set; }
        public int TelMHDiagnosisCategoryId { get; set; }
        public string TelMHDiagnosisCategoryName { get; set; }
        public string TelMHDiagnosisCategoryArabicName { get; set; }
        //public int TelMHDiagnosisSubCategoryId { get; set; }
        //public string TelMHDiagnosisSubCategoryName { get; set; }
        //public string TelMHDiagnosisSubCategoryArabicName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class TelMHMostLikelySubDiagnosisDTO:EntityDTO
    {
        public int TelMHMostLikelyDiagnosisId { get; set; }

        public int TelMHDiagnosisSubCategoryId { get; set; }
        public string TelMHDiagnosisSubCategoryName { get; set; }
        public string TelMHDiagnosisSubCategoryArabicName { get; set; }

    }
}

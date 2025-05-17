using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelMHMostLikelySubDiagnosis:Entity
    {
        public int TelMHMostLikelyDiagnosisId { get; set; }
        public TelMHMostLikelyDiagnosis TelMHMostLikelyDiagnosis { get; set; }

        public int TelMHDiagnosisSubCategoryId { get; set; }
        public TelMHDiagnosisSubCategory TelMHDiagnosisSubCategory { get; set; }


    }
}

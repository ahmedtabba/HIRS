using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelMHDiagnosis:Entity
    {
        public int TelMHClinicalStoryId { get; set; }
        public TelMHClinicalStory TelMHClinicalStory { get; set; }
        public int TelMHDiagnosisCategoryId { get; set; }
        public TelMHDiagnosisCategory TelMHDiagnosisCategory { get; set; }
        //public int TelMHDiagnosisSubCategoryId { get; set; }
        //public TelMHDiagnosisSubCategory TelMHDiagnosisSubCategory { get; set; }
    }
}

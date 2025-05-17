using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelMHMostLikelyDiagnosis:Entity
    {
        public int TelMHDiagnosisCategoryId { get; set; }
        public TelMHDiagnosisCategory TelMHDiagnosisCategory { get; set; }
        public int TelMHClinicalStoryId { get; set; }
        public TelMHClinicalStory TelMHClinicalStory { get; set; }

        public ICollection<TelMHMostLikelySubDiagnosis> TelMHMostLikelySubDiagnoses { get; set; }



        public TelMHMostLikelyDiagnosis()
        {
            TelMHMostLikelySubDiagnoses = new List<TelMHMostLikelySubDiagnosis>();
        }
    }
}

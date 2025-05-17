using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class TelMHMostLikelyDiagnosisDTO : EntityDTO
    {
        public int TelMHDiagnosisCategoryId { get; set; }
        public string TelMHDiagnosisCategoryName { get; set; }
        public string TelMHDiagnosisCategoryArabicName { get; set; }
        public int TelMHClinicalStoryId { get; set; }

        public ICollection<TelMHMostLikelySubDiagnosisDTO> TelMHMostLikelySubDiagnoses { get; set; }
        public List<int> TelMHMostLikelySubDiagnosesIds { get; set; }
        public List<string> TelMHMostLikelySubDiagnosesNames { get; set; }
        public string TelMHMostLikelySubDiagnosesNamesStr { get { return string.Join(",", TelMHMostLikelySubDiagnosesNames); } }
        public string TelMHMostLikelySubDiagnosesArabicNamesStr { get { return string.Join(",", TelMHMostLikelySubDiagnosesArabicNames); } }
        public List<string> TelMHMostLikelySubDiagnosesArabicNames { get; set; }




        public TelMHMostLikelyDiagnosisDTO()
        {
            TelMHMostLikelySubDiagnoses = new List<TelMHMostLikelySubDiagnosisDTO>();
            TelMHMostLikelySubDiagnosesIds = new List<int>();
            TelMHMostLikelySubDiagnosesNames = new List<string>();
            TelMHMostLikelySubDiagnosesArabicNames = new List<string>();
        }
    }
}

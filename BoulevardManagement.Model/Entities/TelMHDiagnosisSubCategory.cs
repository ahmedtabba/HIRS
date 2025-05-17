using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelMHDiagnosisSubCategory:Entity
    {
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public int TelMHDiagnosisCategoryId { get; set; }
        public TelMHDiagnosisCategory TelMHDiagnosisCategory { get; set; }
    }
}

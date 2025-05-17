using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class TelMHDiagnosisSubCategoryDTO:EntityDTO
    {
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public int TelMHDiagnosisCategoryId { get; set; }
    }
}

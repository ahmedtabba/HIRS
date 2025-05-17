using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelMHDiagnosisCategory:Entity
    {
        public string Name { get; set; }
        public string ArabicName { get; set; }
    }
}

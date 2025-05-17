using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelICUInternalConsultationForm : Entity
    {
        public int TeleICUId { get; set; }
        public TeleICU TeleICU { get; set; }
        public string Diagnosis { get; set; }
        public DateTime DateOfSurgery { get; set; }
        public int? UndeAnesthesia{ get; set; }
        public string NameOfDoctor { get; set; }
        public string Recommendations { get; set; }
    }
}

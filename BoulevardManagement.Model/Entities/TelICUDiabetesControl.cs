using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelICUDiabetesControl : Entity
    {
        public int TeleICUId { get; set; }
        public TeleICU TeleICU { get; set; }
        public DateTime Date { get; set; }
        public string Diabetes { get; set; }
        public string DoseOfMedication{ get; set; }
        public string MedicationMethod { get; set; }
        public string Notes { get; set; }
        public string RecommendationByDoctor{ get; set; }

    }
}

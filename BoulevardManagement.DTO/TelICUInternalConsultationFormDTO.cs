using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BoulevardManagement.DTO
{
    public class TelICUInternalConsultationFormDTO : EntityDTO
    {
        public int TeleICUId { get; set; }
        public string Diagnosis { get; set; }
        public DateTime DateOfSurgery { get; set; }
        public UnderAnesthesiaEnum? UndeAnesthesia { get; set; }
        public string NameOfDoctor { get; set; }
        public string Recommendations { get; set; }
        public CaseStatus CaseStatus { get; set; }
        public TelICUInternalConsultationFormDTO()
        {
            DateOfSurgery = DateTime.Now;
        }
    }
}

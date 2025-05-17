using BoulevardManagement.DTO.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Utilities.HashidsNet;
using TripleA.Utility.DataAnnotationsAttributes;

namespace BoulevardManagement.DTO
{
    public class TelICUDiabetesControlDTO : EntityDTO
    {
        public int TeleICUId { get; set; }
        public string EncrptedTeleICUId { get { return HashIdsManager.Encrypt(TeleICUId); } set { } }
        public DateTime Date { get; set; }
        public string Diabetes { get; set; }
        public string DoseOfMedication { get; set; }
        public string MedicationMethod { get; set; }
        public string Notes { get; set; }
        public string RecommendationByDoctor { get; set; }
        public TelICUDiabetesControlDTO()
        {
            Date = DateTime.Now;
        }

    }
}

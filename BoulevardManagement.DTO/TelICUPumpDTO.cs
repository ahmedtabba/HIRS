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
    public class TelICUPumpDTO : EntityDTO
    {
        public int TeleICUId { get; set; }
        public string EncrptedTeleICUId { get { return HashIdsManager.Encrypt(TeleICUId); } set { } }
        public DateTime PumpDate { get; set; }
        public string MedicationName { get; set; }
        public string FlowRate { get; set; }
        public string Notes { get; set; }
        public TelICUPumpDTO()
        {
            PumpDate = DateTime.Now;
        }

    }
}

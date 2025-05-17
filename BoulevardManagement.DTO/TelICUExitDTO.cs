using BoulevardManagement.DTO.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class TelICUExitDTO : EntityDTO
    {

        public int TeleICUId { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string BloodPressureFirstPart { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string BloodPressureSecondPart { get; set; }
        public string BloodPressure { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string Diagnosis { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public decimal? Temperature { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string Temperature_Recovery { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public int? Oxygenation { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public int? Pulse { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string Recommendations { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string Medication { get; set; }

        public CaseStatus CaseStatus { get; set; }

    }
}

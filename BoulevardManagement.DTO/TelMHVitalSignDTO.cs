using BoulevardManagement.DTO.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.DTO
{
    public class TelMHVitalSignDTO:EntityDTO
    {
        public int TeleMentalHealthId { get; set; }
        public string EncrptedTeleMentalHealthId { get { return HashIdsManager.Encrypt(TeleMentalHealthId); } set { } }
        //[Required]
        public string BloodPressure { get; set; }
        public string BloodPressureLang { get {
                if (System.Globalization.CultureInfo.CurrentCulture.TextInfo.IsRightToLeft)
                {
                    return BloodPressure.Replace("/","\\");
                }
                else
                {
                    return BloodPressure;
                }
            } }
        [Required(ErrorMessageResourceType = typeof(VitalSignResource), ErrorMessageResourceName = "BloodPressureFirstPartRequired")]
        public string BloodPressureFirstPart { get; set; }
        [Required(ErrorMessageResourceType = typeof(VitalSignResource), ErrorMessageResourceName = "BloodPressureSecondPartRequired")]
        public string BloodPressureSecondPart { get; set; }

        public decimal? MapBloodPressure { get; set; }
        [Required(ErrorMessageResourceType = typeof(VitalSignResource), ErrorMessageResourceName = "TemperatureRequired")]
        public string Temperature { get; set; }
        [Required(ErrorMessageResourceType = typeof(VitalSignResource), ErrorMessageResourceName = "PulseRequired")]
        public string Pulse { get; set; }
        [Required(ErrorMessageResourceType = typeof(VitalSignResource), ErrorMessageResourceName = "OxygenationRequired")]
        public string Oxygenation { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime TimeOfMeasurement { get; set; }
        [Required(ErrorMessageResourceType = typeof(VitalSignResource), ErrorMessageResourceName = "NameRequired")]
        public string Name { get; set; }


        public TelMHVitalSignDTO()
        {
            TimeOfMeasurement = DateTime.Now;
        }
    }
}

using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelMHVitalSign:Entity
    {
        public int TeleMentalHealthId { get; set; }
        public TeleMentalHealth TeleMentalHealth { get; set; }
        public string BloodPressure { get; set; }
        public decimal? MapBloodPressure { get; set; }
        public string Temperature { get; set; }
        public string Pulse { get; set; }
        public string Oxygenation { get; set; }
        public DateTime TimeOfMeasurement { get; set; }
        public string Name { get; set; }
    }
}

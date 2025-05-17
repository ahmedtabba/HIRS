using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TeleMentalHealthUser:Entity
    {
        public int TeleMentalHealthId { get; set; }
        public string UserId { get; set; }
        public int JobRole { get; set; }

        public TeleMentalHealth TeleMentalHealth { get; set; }
        public string Color { get; set; }
    }
}

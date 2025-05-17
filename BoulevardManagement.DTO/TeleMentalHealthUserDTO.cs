using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class TeleMentalHealthUserDTO:EntityDTO
    {
        public int TeleMentalHealthId { get; set; }
        public string UserId { get; set; }
        public JobRole JobRole { get; set; }
        public string Color { get; set; }
    }
}

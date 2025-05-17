using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TeleMentalHealth:Entity
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int CurrentStep { get; set; }
        public bool Remotely { get; set; }
        public int Status { get; set; }
        public ICollection<TeleMentalHealthUser> InvolvedUsers { get; set; }


        public TeleMentalHealth()
        {
            InvolvedUsers = new List<TeleMentalHealthUser>();
        }
    }
}

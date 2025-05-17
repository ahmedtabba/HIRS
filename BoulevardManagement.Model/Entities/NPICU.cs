using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class NPICU:Entity
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int CurrentStep { get; set; }
        public int NPICUType { get; set; }
        public int Status { get; set; }

        public ICollection<NPICUUser> InvolvedUsers { get; set; }


        public NPICU()
        {
            InvolvedUsers = new List<NPICUUser>();
        }
    }
}

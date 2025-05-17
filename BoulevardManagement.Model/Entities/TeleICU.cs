using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TeleICU : Entity
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int CurrentStep { get; set; }
        public ICollection<TeleICUUser> InvolvedUsers { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public int Status { get; set; }
        public TeleICU()
        {
            InvolvedUsers = new List<TeleICUUser>();

        }

    }
}

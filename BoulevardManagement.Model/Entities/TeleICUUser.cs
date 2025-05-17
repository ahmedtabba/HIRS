using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TeleICUUser : Entity
    {
        public int TeleICUId { get; set; }
        public string UserId { get; set; }
        public int JobRole { get; set; }

        public TeleICU TeleICU { get; set; }
        public string Color { get; set; }
    }
}

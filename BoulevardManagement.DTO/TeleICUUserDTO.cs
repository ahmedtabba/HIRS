using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class TeleICUUserDTO : EntityDTO
    {
        public int TeleICUId { get; set; }
        public string UserId { get; set; }
        public JobRole JobRole { get; set; }
        public string Color { get; set; }
    }
}

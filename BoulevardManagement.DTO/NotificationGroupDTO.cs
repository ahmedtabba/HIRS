using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class NotificationGroupDTO : EntityDTO
    {
        public NotificationGroupDTO()
        {
            GroupNotifications = new List<NotificationDTO>();
        }

        public virtual string Name { get; set; }

        public virtual IList<NotificationDTO> GroupNotifications { get; set; }


    }
}

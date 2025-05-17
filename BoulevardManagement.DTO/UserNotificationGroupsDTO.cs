using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class UserNotificationGroupsDTO : EntityDTO
    {
        public string UserId { get; set; }

        public int NotificationGroupId { get; set; }

        public NotificationGroupDTO NotificationGroup { get; set; }
    }
}

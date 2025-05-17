using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public partial class UserNotificationGroups : Entity
    {
        public string UserId { get; set; }

        public int NotificationGroupId { get; set; }

        public NotificationGroup NotificationGroup { get; set; }


        public UserNotificationGroups()
        {

        }
    }
}

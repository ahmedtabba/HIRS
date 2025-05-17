using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public partial class UserNotification : Entity
    {
        public string UserId { get; set; }

        public int NotificationId { get; set; }

        public virtual Notification Notification { get; set; }

        public string NotificationString { get; set; }

        public string ObjectId { get; set; }

        public int ObjectType { get; set; }

        public bool IsView { get; set; }

        public bool IsUnRead { get; set; }
        public string DiractLink { get; set; }

        public UserNotification()
        {

        }
    }
}

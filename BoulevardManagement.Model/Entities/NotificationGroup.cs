using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public partial class NotificationGroup : Entity
    {
        public NotificationGroup()
        {
            GroupNotifications = new List<Notification>();
        }

        public virtual string Name { get; set; }

        public virtual ICollection<Notification> GroupNotifications { get; set; }
    }
}

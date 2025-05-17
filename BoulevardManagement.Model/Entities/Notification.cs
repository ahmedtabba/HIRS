using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public partial class Notification : Entity
    {
        public Notification()
        {
            Groups = new List<NotificationGroup>();
        }
        public virtual string Name { get; set; }

        public virtual ICollection<NotificationGroup> Groups { get; set; }

    }
}

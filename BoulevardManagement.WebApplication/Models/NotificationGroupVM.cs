using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoulevardManagement.WebApplication.Models
{
    public class NotificationGroupVM
    {
        public NotificationGroupVM()
        {
            Notifications = new List<TreeViewItemModel>();
            FinalSelectedNotifications = new string[] { };
        }

        public string Id { get; set; }
        [Required]
        public virtual string Name { get; set; }

        public List<TreeViewItemModel> Notifications { get; set; }

        public string[] FinalSelectedNotifications { get; set; }
    }
}
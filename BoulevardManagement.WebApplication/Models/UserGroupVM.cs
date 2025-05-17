using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoulevardManagement.WebApplication.Models
{
    public class UserGroupVM
    {
        public UserGroupVM()
        {
            Roles = new List<TreeViewItemModel>();
            FinalSelectedRoles = new string[] { };
        }


        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<TreeViewItemModel> Roles { get; set; }

        public string[] FinalSelectedRoles { get; set; }

    }
}
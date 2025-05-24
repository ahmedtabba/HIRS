using BoulevardManagement.DTO.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace BoulevardManagement.DTO
{
    public class DepartmentTestDTO : EntityDTO
    {
        [Required(ErrorMessageResourceType = typeof(DepartmentTestResource), ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "Name", ResourceType = typeof(DepartmentTestResource))]
        public string Name { get; set; }
        
        [Display(Name = "OpeningDate", ResourceType = typeof(DepartmentTestResource))]
        public DateTime OpeningDate { get; set; }

        [Display(Name = "ResponsibleItUserId", ResourceType = typeof(DepartmentTestResource))]
        public string ResponsibleItUserId { get; set; }
        [Display(Name = "ResponsibleItUserName", ResourceType = typeof(DepartmentTestResource))]
        public string ResponsibleItUserName { get; set; }

    }
}
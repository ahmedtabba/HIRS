using BoulevardManagement.DTO.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace BoulevardManagement.DTO
{
    public class EmployeeTestDTO : EntityDTO
    {
        [Required(ErrorMessageResourceType = typeof(EmployeeTestResource), ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "Name", ResourceType = typeof(EmployeeTestResource))]
        public string Name { get; set; }

        [Display(Name = "BirthDate", ResourceType = typeof(EmployeeTestResource))]
        public DateTime BirthDate { get; set; }

        [Display(Name = "ImageUrl", ResourceType = typeof(EmployeeTestResource))]
        public string ImageUrl { get; set; }

        [Display(Name = "CVUrl", ResourceType = typeof(EmployeeTestResource))]
        public string CVUrl { get; set; }
        
        [Display(Name = "DepartmentName", ResourceType = typeof(EmployeeTestResource))]
        public string DepartmentId { get; set; }
    }
}
using BoulevardManagement.DTO.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class LocationDTO : EntityDTO
    {
        [Required(ErrorMessageResourceType = typeof(LocationResource), ErrorMessageResourceName = "CodeRequired")]
        [Display(Name = "Code", ResourceType = typeof(LocationResource))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceType = typeof(LocationResource), ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "Name", ResourceType = typeof(LocationResource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(LocationResource), ErrorMessageResourceName = "DepartmentRequired")]
        [Display(Name = "Department", ResourceType = typeof(LocationResource))]
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentArabicName { get; set; }

        [Display(Name = "Note", ResourceType = typeof(LocationResource))]
        public string Note { get; set; }
        public bool Occupied { get; set; }
    }
}

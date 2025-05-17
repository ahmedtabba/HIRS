using BoulevardManagement.DTO.Resources;
using System.ComponentModel.DataAnnotations;

namespace BoulevardManagement.DTO
{
    public class ProjectDTO : EntityDTO
    {

        [Display(Name = "Code", ResourceType = typeof(ProjectResource))]
        [Required(ErrorMessageResourceType = typeof(ProjectResource), ErrorMessageResourceName = "CodeRequired")]
        public string Code { get; set; }

        [Display(Name = "Name", ResourceType = typeof(ProjectResource))]
        [Required(ErrorMessageResourceType = typeof(ProjectResource), ErrorMessageResourceName = "NameRequired")]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ProjectResource))]
        public string Description { get; set; }
    }
}

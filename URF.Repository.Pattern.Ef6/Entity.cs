using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System;

namespace Repository.Pattern.Ef6
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }

        [Key]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }
    }
}
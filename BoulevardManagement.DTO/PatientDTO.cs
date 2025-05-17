using BoulevardManagement.DTO.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Utilities;
using TripleA.Utility.DataAnnotationsAttributes;

namespace BoulevardManagement.DTO
{
    public class PatientDTO : EntityDTO
    {
        [Required(ErrorMessageResourceType = typeof(PatientResource), ErrorMessageResourceName = "FirstNameRequired")]
        [Display(Name = "FirstName", ResourceType = typeof(PatientResource))]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceType = typeof(PatientResource), ErrorMessageResourceName = "LastNameRequired")]
        [Display(Name = "LastName", ResourceType = typeof(PatientResource))]
        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }

        [Display(Name = "FatherName", ResourceType = typeof(PatientResource))]
        public string FatherName { get; set; }

        [Display(Name = "MotherName", ResourceType = typeof(PatientResource))]
        public string MotherName { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(PatientResource))]
        [ContactNumber]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(PatientResource), ErrorMessageResourceName = "BirthDateRequired")]
        [Display(Name = "BirthDate", ResourceType = typeof(PatientResource))]
        public DateTime BirthDate { get; set; }

        [Display(Name = "PlaceOfBirth", ResourceType = typeof(PatientResource))]
        public string PlaceOfBirth { get; set; }


        //[Display(Name = "Address", ResourceType = typeof(PatientResource))]
        //public string Address { get; set; }
        [Required(ErrorMessageResourceType = typeof(PatientResource), ErrorMessageResourceName = "GenderRequired")]

        [Display(Name = "Gender", ResourceType = typeof(PatientResource))]
        public Gender Gender { get; set; }
        public string GenderDescription { get { return Gender.GetEnumDescription(); } }

        [Display(Name = "BloodType", ResourceType = typeof(PatientResource))]
        public BloodType? BloodType { get; set; }

        [Display(Name = "MaritalStatus", ResourceType = typeof(PatientResource))]
        public MaritalStatus? MaritalStatus { get; set; }


        //المريض يتبع لقسم
        [Required(ErrorMessageResourceType = typeof(PatientResource), ErrorMessageResourceName = "DepartmentRequired")]
        [Display(Name = "Department", ResourceType = typeof(PatientResource))]
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentArabicName { get; set; }

        public DateTime DateOfCreate { get; set; }
        public int Number { get; set; }
        public string NumberStr { get; set; }
        public string PatientCode { get; set; }

        public PatientDTO()
        {
            BirthDate = DateTime.Now;
        }
    }
}

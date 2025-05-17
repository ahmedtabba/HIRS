using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class Patient : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string PlaceOfBirth { get; set; }
        //public string Address { get; set; }
        public int Gender { get; set; }
        public int? BloodType { get; set; }
        public int? MaritalStatus { get; set; }

        //المريض يتبع لقسم
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int Number { get; set; }
        public string NumberStr { get; set; }

    }
}

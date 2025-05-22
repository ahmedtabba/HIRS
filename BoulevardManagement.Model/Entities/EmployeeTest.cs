using Repository.Pattern.Ef6;
using System;

namespace BoulevardManagement.Model.Entities
{
    public class EmployeeTest : Entity
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string ImageUrl { get; set; }

        public string CVUrl { get; set; }

        public string DepartmentId { get; set; }
    }
}
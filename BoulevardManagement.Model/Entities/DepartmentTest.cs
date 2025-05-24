using Repository.Pattern.Ef6;
using System;

namespace BoulevardManagement.Model.Entities
{
    public class DepartmentTest : Entity
    {
        public string Name { get; set; }

        public DateTime OpeningDate { get; set; }

        public string ResponsibleItUserId { get; set; }
    }
}
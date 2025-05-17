using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class DepartmentDTO : EntityDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public bool HaveLocation { get; set; }
    }
}

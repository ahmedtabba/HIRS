using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class ErrorLog:Entity
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public string UserName { get; set; }
    }
}

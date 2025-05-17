using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class ErrorLogDTO:EntityDTO
    {
        public ErrorLogDTO()
        {
            this.Code = Convert.ToInt32(DateTime.Now.Subtract(new DateTime(2019, 1, 1)).TotalSeconds).ToString("X");
        }

        public string Code { get; set; }
        public string Message { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public string UserName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}

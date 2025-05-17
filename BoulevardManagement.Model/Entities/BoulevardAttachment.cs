using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{

    public class BoulevardAttachment : Entity
    {
        public  string FilePath
        {
            get;
            set;
        }

        public  string Description
        {
            get;
            set;
        }

        public  int ReferenceId
        {
            get;
            set;
        }

        public  int ReferenceType
        {
            get;
            set;
        }
        public string FileExtension { get; set; }

        public string CreatedByName { get; set; }

        public int ReferenceStatus
        {
            get;
            set;
        }
    }
}

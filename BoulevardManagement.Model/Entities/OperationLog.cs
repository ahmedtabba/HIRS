using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class OperationLog:Entity
    {
        public virtual string UserName { get; set; }

        public virtual string EntityType { get; set; }

        public int ObjectId { get; set; }

        public string ObjectRefernceNO { get; set; }

        public int MentionedObjectId { get; set; }

        public string MentionedObjectRefernceNO { get; set; }

        public string MentionedObjectType { get; set; }

        public int OperationType { get; set; }

        public string LogDescription { get; set; }
    }
}

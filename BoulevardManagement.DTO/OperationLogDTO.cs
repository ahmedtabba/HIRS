using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Utilities;

namespace BoulevardManagement.DTO
{
    public class OperationLogDTO:EntityDTO
    {
        public virtual string UserName { get; set; }

        public virtual string EntityType { get; set; }

        public int ObjectId { get; set; }

        public string ObjectRefernceNO { get; set; }

        public int MentionedObjectId { get; set; }

        public string MentionedObjectRefernceNO { get; set; }

        public string MentionedObjectType { get; set; }

        public OperationTypeEnum OperationType { get; set; }

        public DateTime CreationDate { get; set; }

        public string LogDescription { get; set; }
        public string OperationTypeDescription { get { return OperationType.GetEnumDescription(); } }

        public string ReferncedObjectNo
        {
            get
            {
                if (this.OperationType == OperationTypeEnum.Mention)
                    return MentionedObjectRefernceNO;
                else
                    return ObjectRefernceNO;
            }
        }

        public string RefernceLink
        {
            get
            {
                if (this.OperationType == OperationTypeEnum.Mention)
                    return MentionedLink;
                else
                    return ObjectLink;
            }
        }

        public string MentionedBy
        {
            get
            {
               

                return "";
            }
        }

        public string MentionedLink
        {
            get
            {
                
                return "";
            }
        }

        public string ObjectLink
        {
            get
            {
               

                return "";
            }
        }
    }
}

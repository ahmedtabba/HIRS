using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class StickyNote : Entity
    {
        public string ObjectType { get; set; }
        public int ObjectId { get; set; }
        public string ObjectTag { get; set; }
        public string Note { get; set; }
        public int Channel { get; set; }
        public string CreatedUserName { get; set; }
        public int? ParentId { get; set; }
        public string FilePath { get; set; }
        public int StickyNoteMessageType { get; set; }


        public StickyNote()
        {

        }
    }
}

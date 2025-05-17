using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.DTO
{
    public class StickyNoteGridDTO : EntityDTO
    {
        public string ObjectType { get; set; }
        public string ObjectTag { get; set; }
        public int ObjectId { get; set; }
        public string ObjectEncriptedId { get { return HashIdsManager.Encrypt(ObjectId); } set { } }
        public string ObjectLink { get; set; }
        public string Note { get; set; }
        public ChannelEnum Channel { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedUserName { get; set; }

        public DateTime CreationDate { get; set; }

        public string AvatarLetters
        {
            get
            {
                string str = "";
                string[] txtArray = CreatedUserName.Split(' ');
                if (txtArray.Length > 1)
                    str += txtArray.First().Substring(0, 1) + txtArray.Last().Substring(0, 1);
                else
                    str += txtArray.First().Substring(0, 1);
                return str;
            }
        }
    }
}

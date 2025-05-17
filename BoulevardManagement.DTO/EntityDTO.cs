using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Utilities.Encryption;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.DTO
{
    public class EntityDTO
    {
        public int Id { get; set; }

        public string EncrptedId { get { return HashIdsManager.Encrypt(Id); } set { } }

    }
}

using HashidsNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleA.Utilities.HashidsNet
{
    public class HashIdsManager
    {
        static Hashids hashids;
        private static string salt = "Boulevard salt";
        private static string defaultAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        static HashIdsManager()
        {
            hashids = new Hashids(salt, minHashLength: 6, alphabet: defaultAlphabet);
        }

        public static string Encrypt(int id)
        {
            return hashids.Encode(id);
        }

        public static string Encrypt(int? id)
        {
            if (!id.HasValue)
                return string.Empty;
            else
                return hashids.Encode(id.Value);
        }

        public static string Encrypt(string id)
        {
            int intResult;
            Guid guidResult;
            if (int.TryParse(id, out intResult))
                return hashids.Encode(intResult);
            else if (Guid.TryParse(id, out guidResult))
                return id;

            throw new Exception("this [id] is not able to encrypt -> " + id);
        }


        public static int Decrypt(string hashid)
        {
            try
            {
                return hashids.Decode(hashid).First();
            }
            catch 
            {
                return -1;
            }
        }
    }
}

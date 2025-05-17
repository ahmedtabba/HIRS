using System.Configuration;
using System.IO;
using TripleA.Utilities.HashidsNet;

namespace TripleA.Utilities.Helpers
{
    public static class HelperMethods
    {
        #region Create/Delete Folder

        public static string CreateFolder(string folderPath)
        {
            bool exists = Directory.Exists(folderPath);

            if (!exists)
                Directory.CreateDirectory(folderPath);

            return folderPath;
        }


        public static string DeleteFolder(string folderPath)
        {
            bool exists = Directory.Exists(folderPath);

            if (exists)
                Directory.Delete(folderPath, true);

            return folderPath;
        }

        #endregion


        public static string getFullActionURL(string actionUrl, string requestId)
        {
            return ConfigurationManager.AppSettings["SystemURL"] + actionUrl + HashIdsManager.Encrypt(requestId);
        }
        public static string getActionURL(string actionUrl, string requestId)
        {
            return actionUrl + HashIdsManager.Encrypt(requestId);
        }
    }
}

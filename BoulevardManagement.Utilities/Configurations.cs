using System;
using System.Configuration;

namespace BoulevardManagement.Utilities
{
    public static class Configurations
    {
        public static string HtmlTemplatePath
        {
            get { return System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["HtmlTemplatePath"].ToString()); }
        }
        public static string MailPort
        {
            get { return ConfigurationManager.AppSettings["MailPort"].ToString(); }
        }
        public static string MailHost
        {
            get { return ConfigurationManager.AppSettings["MailHost"].ToString(); }
        }
        public static string AdminMail
        {
            get { return ConfigurationManager.AppSettings["AdminMail"].ToString(); }
        }
        public static string AdminPassword
        {
            get { return ConfigurationManager.AppSettings["AdminPassword"].ToString(); }
        }
        public static string TestMail
        {
            get { return ConfigurationManager.AppSettings["TestMail"].ToString(); }
        }
        public static bool InsertIdentitySeedData
        {

            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["InsertIdentitySeedData"].ToString());
            }
        }
        public static string SuperUserEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["SuperUserEmail"].ToString();
            }
        }
        public static string SuperUserFullName
        {
            get
            {
                return ConfigurationManager.AppSettings["SuperUserFullName"].ToString();
            }
        }


        public static string MailImagesPath
        {
            get { return (ConfigurationManager.AppSettings["MailImagesPath"].ToString()); }
        }
        public static string ReportTemplatesPath
        {
            get { return (ConfigurationManager.AppSettings["ReportTemplatesPath"].ToString()); }
        }
        public static string SystemURL
        {
            get
            {
                return ConfigurationManager.AppSettings["SystemURL"].ToString();
            }
        }

        public static string ProfileVirtualPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ProfilePathURL"].ToString();
            }
        }


        public static int MaxDaysToCleanErrorLog
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["MaxDaysToCleanErrorLog"].ToString());
            }
        }

        public static string ProfilePhysicalPath
        {
            get
            {
                return System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ProfilePathURL"].ToString());
            }
        }


        public static string CsvFilePhysicalPath
        {
            //get { return ConfigurationManager.AppSettings["AttachmentsPathURL"].ToString(); }
            get { return System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CsvFilePathURL"].ToString()); }
        }


        #region  Attachments Path
        public static string AttachmentsPhysicalPath
        {
            //get { return ConfigurationManager.AppSettings["AttachmentsPathURL"].ToString(); }
            get { return System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AttachmentsPathURL"].ToString()); }
        }
        #endregion

        #region Temp Attachments Path
        public static string TempAttachmentsPhysicalPath
        {
            //get { return ConfigurationManager.AppSettings["TempAttachmentsPathURL"].ToString(); }
            get { return System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["TempAttachmentsPathURL"].ToString()); }
        }
        #endregion

    }
}

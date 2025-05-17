using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoulevardManagement.WebApplication.Helper
{
    public static class ZLog
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Error(Exception ex)
        {
            logger.Error(ex);
        }
    }
}
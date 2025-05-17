using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace BoulevardManagement.WebApplication
{
    [Obsolete("we have to user Permission instead", true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {



        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { redirectTo = FormsAuthentication.LoginUrl }
                };
            }
            else
            {
                string AccessDeniedURL = string.Format("{0}/Home/AccessDenied", ConfigurationManager.AppSettings["SystemURL"].ToString());

                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    filterContext.Result = new RedirectResult(AccessDeniedURL);
                }
            }

            
        }
    }

}
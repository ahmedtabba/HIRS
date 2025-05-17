using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BoulevardManagement.DTO;
using BoulevardManagement.WebApplication.App_Start;

namespace BoulevardManagement.WebApplication
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        private readonly string[] permissions;
        public PermissionAttribute(params string[] permissions)
        {
            this.permissions = permissions;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool result = false;


            if (!String.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                var userRoles = new ApplicationContext().GetApplicationUserData().Roles;

                foreach (var permission in permissions)
                {
                    if (userRoles.Select(c => c.ToLower()).Contains(permission.ToString().ToLower()))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoulevardManagement.WebApplication
{
    public class LayoutInjecterAttribute : ActionFilterAttribute
    {
        private readonly string _masterName;
        public LayoutInjecterAttribute(string masterName)
        {
            _masterName = masterName;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var result = filterContext.Result as ViewResult;
            if (result != null)
            {
                result.MasterName = _masterName;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var ctx = HttpContext.Current;
            if (ctx.Session["useaname"] == null)
            {
                ctx.Response.Redirect("~/Account/Logout");
            }

            ctx.Session["useaname"] = "WELCOME";
            ctx.Session.Timeout = 1;
        }
    }
}
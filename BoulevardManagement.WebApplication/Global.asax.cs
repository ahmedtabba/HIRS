using BoulevardManagement.BLL.AutoMapperConfiguration;
using BoulevardManagement.DTO;
using BoulevardManagement.WebApplication.Helper;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BoulevardManagement.WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Binders For Multi Choice Enums
            ModelBinders.Binders.Add(typeof(NICUResuscitation), new FlagEnumModelBinder<NICUResuscitation>());
            ModelBinders.Binders.Add(typeof(GeneralAppearance2), new FlagEnumModelBinder<GeneralAppearance2>());

            ModelBinders.Binders.Add(typeof(RiskFactorsEnum), new FlagEnumModelBinder<RiskFactorsEnum>());
            AutoMapperConfiguration.RegisterAllMappers();

        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            var lang = "en-US";
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
                lang = langCookie.Value;

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo(lang);
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo(lang);

            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            if (Context.Items["AjaxPermissionDenied"] is bool)
            {
                Context.Response.StatusCode = 401;
                Context.Response.End();
            }
        }
    }
}

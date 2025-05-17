using System.Web.Optimization;

namespace BoulevardManagement.WebApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new StyleBundle("~/Content/css/styles").Include(
                           //"~/Content/bootstrap.css",
                           "~/Content/font-awesome.min.css",
                            "~/Content/ionicons.min.css",
                            "~/Content/ej/web/default-theme/ej.web.all.min.css",
                            "~/Content/toastr/toastr.min.css",
                           "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/adminlte-RTL").Include(
                "~/Scripts/AdminLTE-RTL/adminlte.min.js",
                "~/Scripts/AdminLTE-RTL/demo.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminlte").Include(
               "~/Scripts/AdminLTE/adminlte.min.js",
               "~/Scripts/AdminLTE/demo.js"));

            bundles.Add(new StyleBundle("~/Content/css/AdminLTE-RTL").Include(
                      "~/Content/AdminLTE-RTL/AdminLTE.min.css",
                      "~/Content/AdminLTE-RTL/skins/skin-blue.min.css"));


            bundles.Add(new StyleBundle("~/Content/css/AdminLTE").Include(
                      "~/Content/AdminLTE/AdminLTE.min.css",
                      "~/Content/AdminLTE/skins/skin-blue.min.css"));





            bundles.Add(new StyleBundle("~/Content/css/kendocss").Include(
                         //"~/Content/kendo/2017.2.621/kendo.bootstrap.min.css",
                         "~/Content/kendo/2017.2.621/kendo.common.min.css",
                          //"~/Content/kendo/2017.2.621/kendo.metro.min.css",
                          "~/Content/kendo/2017.2.621/kendo.custom.css",
                          "~/Content/kendo/2017.2.621/kendo.rtl.min.css"
                //"~/Content/kendo/2017.2.621/kendo.custom.css",
                //"~/Content/kendo/2017.2.621/kendo.rtl.min.css"
                //"~/Content/kendo/2017.2.621/kendo.metro.mobile.min.css"
                ));


            bundles.Add(new ScriptBundle("~/bundles/kendojs").Include(
                "~/Scripts/kendo/2017.2.621/kendo.all.min.js",
                "~/Scripts/kendo/2017.2.621/kendo.aspnetmvc.min.js",
                "~/Scripts/kendo/2017.2.621/jszip.min.js",
                "~/Scripts/toastr/toastr.min.js",
                "~/Scripts/customnew.js"
                ));



        }
    }
}

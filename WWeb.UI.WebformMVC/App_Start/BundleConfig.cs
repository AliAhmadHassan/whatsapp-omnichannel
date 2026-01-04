using System.Web;
using System.Web.Optimization;

namespace WWeb.UI.WebformMVC
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
              .Include("~/Scripts/jquery-{version}.js")
              .Include("~/Scripts/modaldialog.js")
              .Include("~/Scripts/jquery.maskedinput.min.js")
              .Include("~/Scripts/datejs.js")
              .Include("~/Scripts/spin.min.js")
               .Include("~/Scripts/Menu.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui")
                .Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Scripts/jquery.unobtrusive*", "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/highchart")
               .Include("~/Scripts/Highcharts-3.0.1/js/highcharts.js"));

            bundles.Add(new ScriptBundle("~/bundles/wweb")
                 .Include("~/Scripts/footable.all.min.js")
                 .Include("~/Scripts/jquery.filtertable.min.js")
                 .Include("~/Scripts/site.js")
                 );

            bundles.Add(new StyleBundle("~/Content/Css")
                .Include("~/Content/site.css")
                .Include("~/Content/footable.core.min.css")
                .Include("~/Content/footable.silver.min.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include("~/Content/themes/base/all.css"));
            bundles.Add(new StyleBundle("~/Content/popup").Include("~/Content/popup.css"));
        }
    }
}
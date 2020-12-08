using System.Web;
using System.Web.Optimization;

namespace DAN
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/assets/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/assets/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/sitejs").Include(
                        "~/assets/Scripts/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminjs").Include(
                        "~/assets/Scripts/admin.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/assets/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/assets/Scripts/bootstrap.js",
                      "~/assets/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Styles/css").Include(
                      "~/assets/Styles/bootstrap.css",
                      "~/assets/Styles/style.css"));
        }
		
    }
}

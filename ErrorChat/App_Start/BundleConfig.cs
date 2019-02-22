using System.Web.Optimization;

namespace ErrorChat
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/info").Include(
                      "~/Scripts/advertising.js",
                      "~/Scripts/getSendComments.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                     "~/Scripts/jquery.unobtrusive-ajax.min.js",
                     "~/Scripts/searchMediaScrolling.js",
                     "~/Scripts/infoImage.js"));    
        }
    }
}

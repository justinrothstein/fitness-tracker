using System.Web;
using System.Web.Optimization;

namespace FitnessTracker
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.2.4.js",
                        "~/Scripts/jquery.blockUI.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/toastr.js",
                      "~/Scripts/respond.js"));

            // jrothst 8/1/2017 - I needed to use the CssRewriteUrlTransform class so that bootstrap.css has the relative paths to the 'fonts' folder
            // changed to an absolute path. Otherwise, the Glyphicons were not being found in the 'fonts' folder.
            // 
            // NOTE: MVC will only execute the IItemTransform code ONLY IF A '.min' CSS DOES NOT EXIST. For example, I had to remove the bootstrap.min.css file
            // so that the boostrap.css file would be propertly updated by the IItemTransform code. 
            IItemTransform cssFixer = new CssRewriteUrlTransform();

            bundles.Add(new StyleBundle("~/Content/css/bootstrapCss").Include(
                      "~/Content/css/bootstrap.css", cssFixer));

            bundles.Add(new StyleBundle("~/Content/css/css").Include(
                      "~/Content/css/toastr.css",
                      "~/Content/css/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                    "~/Scripts/angular.min.js",
                    "~/Scripts/angular-route.min.js"));
        }
    }
}

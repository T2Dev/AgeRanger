using System.Web;
using System.Web.Optimization;

namespace AgeRanger
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
			bundles.Add(new ScriptBundle("~/bundles/angular").Include("~/Scripts/angular.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

			bundles.Add(new StyleBundle("~/Content/home").Include("~/Content/home.css"));
			bundles.Add(new StyleBundle("~/Scripts/home").Include("~/Scripts/home.js"));
		}
    }
}

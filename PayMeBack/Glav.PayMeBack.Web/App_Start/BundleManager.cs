using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Glav.PayMeBack.Web
{
    public static class BundleManager
    {
        public static void RegisterJsBundles(BundleCollection bundles)
        {
			var coreBundle = new ScriptBundle("~/CoreJs");
			coreBundle.Include("~/Scripts/nyroModal/js/jquery.nyroModal.js")
                .Include("~/Scripts/knockout-2.0.0.js")
                .Include("~/Scripts/custom/payMeBack.Core.js")
                .Include("~/Scripts/custom/InputManager.js")
                .Include("~/Scripts/custom/progressManager.js")
                .Include("~/Scripts/custom/notificationEngine.js")
                .Include("~/Scripts/custom/Login.js");
			bundles.Add(coreBundle);

			var landingPageBundle = new ScriptBundle("~/LandingPageJs")
                .Include("~/Scripts/custom/landing-page.js");
            bundles.Add(landingPageBundle);

			var membershipPageBundle = new ScriptBundle("~/MembershipPageJs")
                .Include("~/Scripts/custom/membership.js");
            bundles.Add(membershipPageBundle);

            var summaryPageBundle = new ScriptBundle("~/SummaryPageJs")
                .Include("~/Scripts/custom/debt-manager.js")
                .Include("~/Scripts/custom/summary-page.js");
            bundles.Add(summaryPageBundle);
        }

        public static void RegisterCssBundles(BundleCollection bundles)
        {
            var cssBundle = new StyleBundle("~/MainCss")
                .Include("~/Content/main.css");
            bundles.Add(cssBundle);

            var nyroModalCssBundle = new StyleBundle("~/NyroModalCss")
                .Include("~/Scripts/nyroModal/styles/nyroModal.css");
            bundles.Add(nyroModalCssBundle);
        }
    }
}
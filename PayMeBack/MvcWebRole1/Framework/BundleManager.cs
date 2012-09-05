using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Glav.PayMeBack.Web.Framework
{
    public static class BundleManager
    {
        public static void RegisterJsBundles()
        {
			var coreBundle = new Bundle("~/CoreJs");
			coreBundle.AddFile("~/Scripts/nyroModal/js/jquery.nyroModal.js");
            coreBundle.AddFile("~/Scripts/knockout-2.0.0.js");
            coreBundle.AddFile("~/Scripts/custom/payMeBack.Core.js");
            coreBundle.AddFile("~/Scripts/custom/InputManager.js");
            coreBundle.AddFile("~/Scripts/custom/progressManager.js");
            coreBundle.AddFile("~/Scripts/custom/notificationEngine.js");
            coreBundle.AddFile("~/Scripts/custom/Login.js");
			BundleTable.Bundles.Add(coreBundle);

			var landingPageBundle = new Bundle("~/LandingPageJs");
            landingPageBundle.AddFile("~/Scripts/custom/landing-page.js");
			BundleTable.Bundles.Add(landingPageBundle);

			var membershipPageBundle = new Bundle("~/MembershipPageJs");
            membershipPageBundle.AddFile("~/Scripts/custom/membership.js");
			BundleTable.Bundles.Add(membershipPageBundle);

            var summaryPageBundle = new Bundle("~/SummaryPageJs");
            summaryPageBundle.AddFile("~/Scripts/custom/debt-manager.js");
            summaryPageBundle.AddFile("~/Scripts/custom/summary-page.js");
            BundleTable.Bundles.Add(summaryPageBundle);
        }

        public static void RegisterCssBundles()
        {
            var cssBundle = new Bundle("~/MainCss");
            cssBundle.AddFile("~/Content/main.css");
            BundleTable.Bundles.Add(cssBundle);

            var nyroModalCssBundle = new Bundle("~/NyroModalCss");
            nyroModalCssBundle.AddFile("~/Scripts/nyroModal/styles/nyroModal.css");
            BundleTable.Bundles.Add(nyroModalCssBundle);
        }
    }
}
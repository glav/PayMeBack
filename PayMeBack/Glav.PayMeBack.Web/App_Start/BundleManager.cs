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
                .Include("~/Scripts/angular.js")
                .Include("~/Scripts/custom/payMeBack.Core.js")
                .Include("~/Scripts/AngularScripts/bootstrap.js")
                .Include("~/Scripts/AngularScripts/userFactory.js")
                .Include("~/Scripts/AngularScripts/debtFactory.js")
                .Include("~/Scripts/AngularScripts/signInController.js")
                .Include("~/Scripts/AngularScripts/addDebtController.js")
                .Include("~/Scripts/AngularScripts/debtPaymentController.js")
                .Include("~/Scripts/AngularScripts/summaryActionLinkController.js")
                .Include("~/Scripts/AngularScripts/authenticateController.js")
                .Include("~/Scripts/AngularScripts/routeConfig.js")
                .Include("~/Scripts/custom/InputManager.js")
                .Include("~/Scripts/custom/progressManager.js")
                .Include("~/Scripts/custom/notificationEngine.js")
                .Include("~/Scripts/custom/ajaxManager.js")
                .Include("~/Scripts/custom/Login.js");
			bundles.Add(coreBundle);

			var landingPageBundle = new ScriptBundle("~/LandingPageJs")
                .Include("~/Scripts/AngularScripts/homeController.js");
            bundles.Add(landingPageBundle);

			var membershipPageBundle = new ScriptBundle("~/MembershipPageJs")
                .Include("~/Scripts/custom/membership.js");
            bundles.Add(membershipPageBundle);

            var summaryPageBundle = new ScriptBundle("~/SummaryPageJs")
                .Include("~/Scripts/AngularScripts/summaryController.js")
                .Include("~/Scripts/custom/debt-manager.js")
                .Include("~/Scripts/custom/notification-manager.js")
                .Include("~/Scripts/custom/accountSettingsManager.js");
            bundles.Add(summaryPageBundle);
        }

        public static void RegisterCssBundles(BundleCollection bundles)
        {
            var cssBundle = new StyleBundle("~/Content/MainCss")
                .Include("~/Content/main.css")
                .Include("~/Content/notification-options.css")
                .Include("~/Content/debt.css");
            bundles.Add(cssBundle);

            var nyroModalCssBundle = new StyleBundle("~/Content/Nyromodal/Styles/NyroModalCss")
                .Include("~/Scripts/nyroModal/styles/nyroModal.css");
            bundles.Add(nyroModalCssBundle);
        }
    }
}
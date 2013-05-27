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
                .Include("~/Scripts/AngularScripts/ui-bootstrap-tpls-0.3.0.js")
                .Include("~/Scripts/AngularScripts/init.js")
                .Include("~/Scripts/AngularScripts/Factories/debtConstantsFactory.js")
                .Include("~/Scripts/AngularScripts/Factories/userFactory.js")
                .Include("~/Scripts/AngularScripts/Factories/eventFactory.js")
                .Include("~/Scripts/AngularScripts/Factories/debtFactory.js")
                .Include("~/Scripts/AngularScripts/Directives/defaultButtonDirective.js")
                .Include("~/Scripts/AngularScripts/Directives/dateValidationDirective.js")
                .Include("~/Scripts/AngularScripts/Factories/notificationFactory.js")
                .Include("~/Scripts/AngularScripts/Controllers/signInController.js")
                .Include("~/Scripts/AngularScripts/Controllers/addDebtController.js")
                .Include("~/Scripts/AngularScripts/Controllers/debtPaymentController.js")
                .Include("~/Scripts/AngularScripts/Controllers/summaryActionLinkController.js")
                .Include("~/Scripts/AngularScripts/Controllers/editDebtController.js")
                .Include("~/Scripts/AngularScripts/Controllers/authenticateController.js")
                .Include("~/Scripts/AngularScripts/Controllers/notificationOptionsController.js")
                .Include("~/Scripts/AngularScripts/Controllers/accountSettingsController.js")
                .Include("~/Scripts/AngularScripts/routeConfig.js")
                .Include("~/Scripts/custom/InputManager.js")
                .Include("~/Scripts/custom/progressManager.js")
                .Include("~/Scripts/custom/notificationEngine.js")
                .Include("~/Scripts/custom/ajaxManager.js")
                .Include("~/Scripts/custom/Login.js");
			bundles.Add(coreBundle);

			var landingPageBundle = new ScriptBundle("~/LandingPageJs")
                .Include("~/Scripts/AngularScripts/Controllers/homeController.js");
            bundles.Add(landingPageBundle);

			var membershipPageBundle = new ScriptBundle("~/MembershipPageJs")
                .Include("~/Scripts/custom/membership.js");
            bundles.Add(membershipPageBundle);

            var summaryPageBundle = new ScriptBundle("~/SummaryPageJs")
                .Include("~/Scripts/AngularScripts/Controllers/summaryController.js")
                .Include("~/Scripts/custom/debt-manager.js");
            bundles.Add(summaryPageBundle);
        }

        public static void RegisterCssBundles(BundleCollection bundles)
        {
            var cssBundle = new StyleBundle("~/Content/MainCss")
                .Include("~/Content/main.css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/notification-options.css")
                .Include("~/Content/debt.css");
            bundles.Add(cssBundle);

            var nyroModalCssBundle = new StyleBundle("~/Content/Nyromodal/Styles/NyroModalCss")
                .Include("~/Scripts/nyroModal/styles/nyroModal.css");
            bundles.Add(nyroModalCssBundle);
        }
    }
}
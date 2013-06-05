/* File Created: July 31, 2012 */

if (typeof window.payMeBack === 'undefined') {
	window.payMeBack = { };
}
if (typeof window.payMeBack.core === 'undefined') {

    window.payMeBack.core = {
        dependencies: {
            moduleName: "paymeback",
            debtConstantsFactory: "debtConstantsFactory",
            userFactory: "userFactory",
            eventFactory: "eventFactory",
            debtFactory: "debtFactory",
            notificationFactory: "notificationFactory",
            homeController: "homeController",
            summaryController: "summaryController",
            signInController: "signInController",
            addDebtController: "addDebtController",
            userAccountController: "userAccountController",
            debtPaymentController: "debtPaymentController",
            summaryActionLinkController: "summaryActionLinkController",
            authenticateController: "authenticateController",
            editDebtController: "editDebtController",
            notificationOptionsController: "notificationOptionsController",
            accountSettingsController: "accountSettingsController"
        },
        rootPath: "/",
        makePathFromVirtual: function(virtualPath) {
            if (typeof virtualPath === 'undefined') {
                return;
            }
            if (virtualPath.indexOf("~") !== 0) {
                return virtualPath;
            }

            var actualPath = virtualPath.replace("~",window.payMeBack.core.rootPath).replace("//","/");
            return actualPath;
        },
        
        
        formatCurrency: function(amount) {
            try {
                var amt = parseFloat(amount);
                var text = "$" + amt;
                if (text.indexOf(".") === -1) {
                    text += ".00";
                }
                return text;
            } catch (e) {
                return "$0.00";
            }
        }
    };
}

if (typeof window.payMeBack.core.colours === 'undefined') {
	window.payMeBack.core.colours = { };
}

/** Colour constants used in dynamic invocation and application ***/
window.payMeBack.core.colours.nyroModalBackground = "#A8A5A5";
window.payMeBack.core.colours.statusMessageInfoBackground = "#A8A5A5";
window.payMeBack.core.colours.statusMessageErrorBackground = "#A8A5A5";
/******************************************************************/

/***************** Authentcation and authorisation *********************/
if (typeof window.payMeBack.auth === 'undefined') {
    window.payMeBack.auth = {};
}

window.payMeBack.auth.accessToken = "";

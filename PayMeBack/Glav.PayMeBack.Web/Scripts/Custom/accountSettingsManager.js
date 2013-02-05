/// <reference path="_references.js" />
/// <reference path="ajaxManager.js" />
/// <reference path="notificationEngine.js" />

if (typeof window.payMeBack.accountSettingsManager === 'undefined') {
    window.payMeBack.accountSettingsManager = {};
}

window.payMeBack.accountSettingsManager = (function () {

    var showAccountSettingsForUser = function(competedCallback) {
        $.nyroModalManual({
            url: '#account-settings-modal',
            minHeight: 300,
            height: 400,
            minWwidth: 500,
            bgColor: window.payMeBack.core.colours.nyroModalBackground,
            //modal: true,
            //closeButton: null,
            endRemove: function () {
                $("#account-settings-container fieldset").show();
                $("#account-settings-container .progress-indicator").hide();
                if (typeof completionCallback !== 'undefined') {
                    completionCallback();
                }

            },
            endShowContent: function () {
                var debtContainer = $("#account-settings-container");
                $(".progress-indicator", debtContainer).hide();
                $("fieldset ul li input", debtContainer).unbind().on("keypress", function (e) {
                    if (e.which === 13) {
                        captureAccountSettingsFormAndSubmit();
                    }
                });
                $("#account-settings-submit").unbind().on("click", function (e) {
                    captureAccountSettingsFormAndSubmit();
                });

            }
        });
    };
    
    return {
        showAccountSettingsForUser: showAccountSettingsForUser
    };
})();
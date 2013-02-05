/// <reference path="_references.js" />
/// <reference path="ajaxManager.js" />
/// <reference path="notificationEngine.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.notificationManager = {};
}

window.payMeBack.notificationManager = (function () {

    var showNotificationOptionsForDebt = function(competedCallback) {
        $.nyroModalManual({
            url: '#notification-options-modal',
            minHeight: 300,
            height: 400,
            minWwidth: 500,
            bgColor: window.payMeBack.core.colours.nyroModalBackground,
            //modal: true,
            //closeButton: null,
            endRemove: function () {
                $("#notification-options-container fieldset").show();
                $("#notification-options-container .progress-indicator").hide();
                if (typeof completionCallback !== 'undefined') {
                    completionCallback();
                }

            },
            endShowContent: function () {
                var debtContainer = $("#notification-options-container");
                $(".progress-indicator", debtContainer).hide();
                $("fieldset ul li input", debtContainer).unbind().on("keypress", function (e) {
                    if (e.which === 13) {
                        captureNotificationFormAndSubmit();
                    }
                });
                $("#notification-options-submit").unbind().on("click", function (e) {
                    captureNotificationFormAndSubmit();
                });

                $("#add-debt-user-email").focus();
            }
        });
        alert('getting there, but this section is not complete');
    };
    
    return {
        showNotificationOptionsForDebt: showNotificationOptionsForDebt
    };
})();
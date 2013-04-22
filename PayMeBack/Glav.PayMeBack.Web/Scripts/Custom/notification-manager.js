/// <reference path="_references.js" />
/// <reference path="ajaxManager.js" />
/// <reference path="notificationEngine.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.notificationManager = {};
}

window.payMeBack.notificationManager = (function () {

    var bindNotificationOptionTriggers = function () {
        $("#debt-notification-interval").unbind().on('change', refreshNotificationDisplay);
        $("#debt-notification-method").unbind().on('change', refreshNotificationDisplay);
    };

    var refreshNotificationDisplay = function () {
        var interval = parseInt($("#debt-notification-interval").val(),10);
        var method = parseInt($("#debt-notification-method").val(),10);

        var frequencyContainer = $("#notification-options-container ul li.frequency-option");
        var emailEntry = $("#notification-options-container li.notification-email-option");
        var smsEntry = $("#notification-options-container li.notification-sms-option");

        if (interval && interval > 0) {
            frequencyContainer.show();
        } else {
            frequencyContainer.hide();
        }

        if (method && method > 0) {
            switch (method) {
                case 1:
                    emailEntry.show();
                    smsEntry.hide();
                    break;
                case 2:
                    smsEntry.show();
                    emailEntry.hide();
                    break;
            }
        } else {
            emailEntry.hide();
            smsEntry.hide();
        }
    }

    var captureNotificationFormAndSubmit = function () {
        alert('not done yet');
    };

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
                var container = $("#notification-options-container");
                $(".progress-indicator", container).hide();
                $("fieldset ul li input", container).unbind().on("keypress", function (e) {
                    if (e.which === 13) {
                        captureNotificationFormAndSubmit();
                    }
                });
                $("#notification-options-submit").unbind().on("click", function (e) {
                    captureNotificationFormAndSubmit();
                });

                bindNotificationOptionTriggers();
                refreshNotificationDisplay();
                $("#add-debt-user-email").focus();

            }
        });
    };

    return {
        showNotificationOptionsForDebt: showNotificationOptionsForDebt
    };
})();
/// <reference path="_references.js" />
/// <reference path="ajaxManager.js" />
/// <reference path="notificationEngine.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.debtManager = {};
}

window.payMeBack.debtManager = (function () {

    var _defaultDateFormat = "d/m/y";

    var showAddDebtDialog = function (completionCallback) {

        $.nyroModalManual({
            url: '#add-debt-modal',
            minHeight: 300,
            height: 400,
            minWwidth: 500,
            bgColor: window.payMeBack.core.colours.nyroModalBackground,
            //modal: true,
            //closeButton: null,
            endRemove: function () {
                $("#add-debt-container fieldset").show();
                $("#add-debt-container .progress-indicator").hide();
                if (typeof completionCallback !== 'undefined') {
                    completionCallback();
                }

            },
            endShowContent: function () {
                var debtContainer = $("#add-debt-container");
                $(".progress-indicator", debtContainer).hide();

                $("#add-debt-user-email").focus();
                //window.payMeBack.inputManager.maskAllMoneyInputControls();
            }
        });
    };

    var invokeCallback = function (callback) {
        if (typeof callback !== 'undefined') {
            callback();
        }
    };


    var showAddPaymentToDebtDialog = function (xPos, yPos) {
        // position and show the dialog
        var container = $("#add-debt-payment-container");
        $("fieldset", container).show();
        container.css('left', xPos + 'px')
            .css('top', yPos + 'px')
            .fadeIn('normal', function () {
                // If the user clicks outside the dialog on the body somewhere, the close the dialog
                $("#add-debt-payment-close").on('click', function (e) {
                    container.fadeOut('normal', function () {
                        $(this).fadeOut();
                    });
                });

                $("#payment-amount").focus();
            });
    };



    var editDebt = function (debtId, completionCallback) {
        $.nyroModalManual({
            url: '#edit-debt-modal',
            minHeight: 320,
            height: 470,
            minWwidth: 500,
            bgColor: window.payMeBack.core.colours.nyroModalBackground,
            endRemove: function () {
                var fieldForm = $("#edit-debt-container fieldset");
                fieldForm.hide();
                $("#edit-debt-container .progress-indicator").hide();
                if (typeof completionCallback !== 'undefined') {
                    completionCallback();
                }

            },
            endShowContent: function () {
                var debtContainer = $("#edit-debt-container");
                $(".progress-indicator", debtContainer).hide();
                $("#edit-debt-container fieldset").removeClass("hidden").fadeIn();
            }
        });
    };

    return {
        showAddDebtDialog: function (completionCallback) { showAddDebtDialog(completionCallback); },
        editDebt: function (debtId, completionCallback) { editDebt(debtId, completionCallback); },
        showAddPaymentToDebtDialog: function (debtId, xPos, yPos, completionCallback) { showAddPaymentToDebtDialog(debtId, xPos, yPos, completionCallback); }
    };
})();
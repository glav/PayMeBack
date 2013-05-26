/// <reference path="_references.js" />
/// <reference path="ajaxManager.js" />
/// <reference path="notificationEngine.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.debtManager = {};
}

window.payMeBack.debtManager = (function () {

    var _defaultDateFormat = "d/m/y";

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




    return {
        showAddPaymentToDebtDialog: function (debtId, xPos, yPos, completionCallback) { showAddPaymentToDebtDialog(debtId, xPos, yPos, completionCallback); }
    };
})();
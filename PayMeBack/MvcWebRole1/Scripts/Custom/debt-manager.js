﻿/// <reference path="_references.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.debtManager = {};
}

window.payMeBack.debtManager = (function () {

    var captureFormData = function () {
        return {
            emailAddress: $("#add-debt-user-email").val(),
            amountOwed: $("#add-debt-amount").val(),
            debtReason: $("#add-debt-reason").val(),
            initialAmountPaid: $("#add-debt-initial-amount").val(),
            notes: $("#add-debt-notes").val(),
            paymentPeriod: $("#add-debt-payment-period").val(),
            expectedEndDate: $("#add-debt-end-date").val()
        };
    };

    var captureDebtFormAndSubmit = function () {
        var debtData = captureFormData();

        $("#add-debt-container fieldset").fadeOut('normal', function () {
            window.payMeBack.progressManager.showProgressIndicator("add-debt-container");
            $.ajax({
                url: window.payMeBack.core.makePathFromVirtual("~/debt/Add"),
                type: "POST",
                data: JSON.stringify(debtData),
                contentType: 'application/json',
                dataType: "json",
                success: function (result) {
                    window.payMeBack.progressManager.hideProgressIndicator("add-debt-container", function () {
                        if (result && typeof result.error !== 'undefined') {
                            window.payMeBack.notificationEngine.showStatusBarMessage("The request had an error: " + result.error, window.payMeBack.notificationEngine.MessageTypeError, "#nyroModalContent", 5);
                            $("#add-debt-container fieldset").fadeIn();

                        } else {
                            if (result && typeof result.success !== 'undefined' && result.success === true) {
                                // This worked
                                $.nyroModalRemove();
                            } else {
                                var msg = "";
                                if (typeof result.error !== 'undefined') {
                                    msg = result.error;
                                }

                                msg = "Sorry, there was an error adding the debt to the system. " + msg;
                                $("#add-debt-container fieldset").fadeIn();

                                window.payMeBack.notificationEngine.showStatusBarMessage(msg, window.payMeBack.notificationEngine.MessageTypeError, "#nyroModalContent");
                            }

                        }
                    });
                },
                error: function () {
                    window.payMeBack.progressManager.hideProgressIndicator("add-debt-container", function () {
                        $("#add-debt-container fieldset").fadeIn();

                        var msg = "There was a problem addin the debt record to the system. Please try again.";
                        window.payMeBack.notificationEngine.showStatusBarMessage(msg, window.payMeBack.notificationEngine.MessageTypeError, "#nyroModalContent", 5);
                    });
                }
            });
        });
    };

    var showAddDebtDialog = function () {
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
            },
            endShowContent: function () {
                $("#add-debt-container fieldset ul li input").unbind().on("keypress", function (e) {
                    if (e.which === 13) {
                        captureDebtFormAndSubmit();
                    }
                });
                $("#add-debt-submit").unbind().on("click", function (e) {
                    captureDebtFormAndSubmit();
                });

                $("#add-debt-user-email").focus();
            }
        });
    };

    return {
        showAddDebtDialog: function () { showAddDebtDialog(); }
    };
})();
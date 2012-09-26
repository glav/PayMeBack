﻿/// <reference path="_references.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.debtManager = {};
}

window.payMeBack.debtManager = (function () {

    var paymentTypes = {
        Unknown: 0,
        Cash: 1,
        BankTransfer: 2,
        Services: 3,
        Goods: 4
    };

    $.ajaxSetup({
        // Disable caching of AJAX responses
        cache: false
    });

    var clearFormData = function () {
        $("#add-debt-container fieldset input[type!='button']").val("");
    }

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
            window.payMeBack.ajaxManager.ajaxRequest("~/debt/Add", "POST", debtData, "add-debt-container", "#nyroModalContent",
                function () {
                    getDebtSummaryHtml();
                    $.nyroModalRemove();
                },
                function () {
                    $("#add-debt-container fieldset").fadeIn();
                });
        });
    };

    var showAddDebtDialog = function (completionCallback) {
        clearFormData();
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
                $("fieldset ul li input", debtContainer).unbind().on("keypress", function (e) {
                    if (e.which === 13) {
                        captureDebtFormAndSubmit();
                    }
                });
                $("#add-debt-submit").unbind().on("click", function (e) {
                    captureDebtFormAndSubmit();
                });

                $("#add-debt-user-email").focus();
                window.payMeBack.inputManager.maskAllMoneyInputControls();
            }
        });
    };

    var getDebtSummaryHtml = function () {
        $.ajax({
            url: window.payMeBack.core.makePathFromVirtual("~/summary/DebtsOwedToMe"),
            type: "GET",
            success: function (result) {
                window.payMeBack.progressManager.hideProgressIndicator("debt-summary-section", function () {
                    if (typeof result !== 'undefined') {
                        $("#debt-summary-section").empty().html(result);
                        // This worked
                    }

                });
            },
            error: function (e) {
                window.payMeBack.progressManager.hideProgressIndicator("debt-summary-section", function () {
                    var msg = "There was a problem retrieving the debt summary data. Please try again.";
                    window.payMeBack.notificationEngine.showStatusBarMessage(msg, window.payMeBack.notificationEngine.MessageTypeError);
                });
            }
        });

    };

    var showAddPaymentToDebtDialog = function (debtId, xPos, yPos, completionCallback) {
        // position and show the dialog
        $("#add-debt-payment-container")
                .css('left', xPos + 'px')
                .css('top', yPos + 'px')
                .fadeIn();
        // If the user clicks outside the dialog on the body somewhere, the close the dialog
        $("body, #add-debt-payment-close").on('click', function (e) {
            $("#add-debt-payment-container").fadeOut('normal', function () {
                $(this).fadeOut();
                if (typeof completionCallback !== 'undefined') {
                    completionCallback();
                }
            });
        });
        // If the user click *inside* the dialog, which is still part of the body, dont let the
        // event propagate otherwise the previous handler will close the dialog
        $("#add-debt-payment-container").on('click', function (e) {
            e.preventDefault();
            e.stopImmediatePropagation();
        });
        $("#payment-amount").focus();
        //alert('adding to debtId:' + debtId + ' - Amount:' + amount + ' - date: ' + dateOfPayment + '  [NOT COMPLETE]');
    };

    var deleteDebt = function (debtId) {
        window.payMeBack.ajaxManager.ajaxRequest("~/debt/Delete?debtId=" + debtId, "DELETE", "", null, null,
            function (result) {
                $('#debt-summary-owed tr[data-debt-id="' + debtId + '"]').fadeOut('normal', function () {
                    $(this).remove();
                });
                if (result && result.total) {
                    $("#debts-owed-to-me span.total-owed-amount").text(result.total);
                }
            },
            function () {
                // error - dont do anything
            });
    };

    var editDebt = function (debtId) {
        alert('not implemented - edit debt Id: ' + debtId);
    };

    return {
        showAddDebtDialog: function (completionCallback) { showAddDebtDialog(completionCallback); },
        deleteDebt: function (debtId) { deleteDebt(debtId); },
        editDebt: function (debtId) { editDebt(debtId); },
        showAddPaymentToDebtDialog: function (debtId, xPos, yPos, completionCallback) { showAddPaymentToDebtDialog(debtId, xPos, yPos, completionCallback); }
    };
})();
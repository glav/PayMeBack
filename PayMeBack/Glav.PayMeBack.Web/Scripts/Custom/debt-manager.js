/// <reference path="_references.js" />

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
                }, "There was a problem adding the debt record to the system. Please try again.");
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

    var invokeCallback = function (callback) {
        if (typeof callback !== 'undefined') {
            callback();
        }
    }

    var captureAddPaymentFormAndSubmit = function (debtId, completionCallback) {
        var payment = {
            debtId: debtId,
            amount: 0,
            date: '',
            paymentType: 1
        };
        payment.amount = $("#payment-amount").val();
        payment.date = $("#payment-date").val();
        payment.paymentType = $("#payment-type").val();

        $("#add-debt-payment-container fieldset").fadeOut('normal', function () {
            window.payMeBack.ajaxManager.ajaxRequest("~/debt/AddPayment", "POST", payment, "add-debt-payment-section", "#add-debt-payment-section",
                function (result) {
                    $("#add-debt-payment-container").fadeOut('normal', function () {
                        invokeCallback(completionCallback);
                    });
                    getDebtSummaryHtml();
                },
                function () {
                    $("#add-debt-payment-container fieldset").fadeIn();
                }, "There was a problem adding a payment", window.payMeBack.notificationEngine.MessageTypeSmallError);
        });
    }

    var showAddPaymentToDebtDialog = function (debtId, xPos, yPos, completionCallback) {
        // position and show the dialog
        var container = $("#add-debt-payment-container");
        $("fieldset", container).show();
        container.css('left', xPos + 'px')
                .css('top', yPos + 'px')
                .fadeIn();
        // If the user clicks outside the dialog on the body somewhere, the close the dialog
        $("body, #add-debt-payment-close").on('click', function (e) {
            container.fadeOut('normal', function () {
                $(this).fadeOut();
                invokeCallback(completionCallback);
            });
        });
        // If the user click *inside* the dialog, which is still part of the body, dont let the
        // event propagate otherwise the previous handler will close the dialog
        container.unbind().on('click', function (e) {
            e.stopImmediatePropagation();
        });;

        $("fieldset ul li input, fieldset ul li select", container).unbind().on("keydown", function (e) {
            if (e.which === 13) {
                captureAddPaymentFormAndSubmit(debtId, completionCallback);
            }
            if (e.which === 27) {
                container.fadeOut();
                invokeCallback(completionCallback);
            }
        });

        $("#payment-amount").focus();
        $("#add-payment-action").unbind().on("click", function () {
            captureAddPaymentFormAndSubmit(debtId, completionCallback);
        });
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

    var getDebtDetails = function () {
        alert('access paymentplan and populate fields');
    }

    var editDebt = function (debtId, completionCallback) {
        $.nyroModalManual({
            url: '#edit-debt-modal',
            minHeight: 300,
            height: 400,
            minWwidth: 500,
            bgColor: window.payMeBack.core.colours.nyroModalBackground,
            endRemove: function () {
                $("#edit-debt-container fieldset").show();
                $("#edit-debt-container .progress-indicator").hide();
                if (typeof completionCallback !== 'undefined') {
                    completionCallback();
                }

            },
            endShowContent: function () {
                var debtContainer = $("#edit-debt-container");
                getDebtDetails();
                $(".progress-indicator", debtContainer).hide();
                $("fieldset ul li input", debtContainer).unbind().on("keypress", function (e) {
                    if (e.which === 13) {
                        alert('not done');
                    }
                });
                $("#edit-debt-submit").unbind().on("click", function (e) {
                    alert('not done');
                });

                window.payMeBack.inputManager.maskAllMoneyInputControls();
            }
        });
    };

    return {
        showAddDebtDialog: function (completionCallback) { showAddDebtDialog(completionCallback); },
        deleteDebt: function (debtId) { deleteDebt(debtId); },
        editDebt: function (debtId, completionCallback) { editDebt(debtId, completionCallback); },
        showAddPaymentToDebtDialog: function (debtId, xPos, yPos, completionCallback) { showAddPaymentToDebtDialog(debtId, xPos, yPos, completionCallback); }
    };
})();
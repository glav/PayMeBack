/// <reference path="_references.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.debtManager = {};
}

window.payMeBack.debtManager = (function () {

    $.ajaxSetup({
        // Disable caching of AJAX responses
        cache: false
    });

    var clearFormData = function () {
        $("#add-debt-container fieldset input").val("");
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

    var showAddDebtDialog = function () {
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

    var deleteDebt = function (debtId) {
        window.payMeBack.ajaxManager.ajaxRequest("~/debt/Delete?debtId="+debtId, "DELETE", "", "debt-summary-owed", null,
            function () {
                $('#debt-summary-owed tr[data-debt-id="' + debtId + '"]').remove();
            },
            function () {
                // error - dont do anything
            });
    };

    var editDebt = function (debtId) {
        alert('not implemented - edit debt Id: ' + debtId);
    };

    return {
        showAddDebtDialog: function () { showAddDebtDialog(); },
        deleteDebt: function (debtId) { deleteDebt(debtId); },
        editDebt: function (debtId) { editDebt(debtId); }
    };
})();
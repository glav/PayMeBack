/// <reference path="_references.js" />
/// <reference path="ajaxManager.js" />
/// <reference path="notificationEngine.js" />

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

    var getPaymentTypeDescrption =  function(paymentTypeValue) {
        switch (paymentTypeValue) {
            case paymentTypes.Unknown:
                return "Unknown";
                break;
            case paymentTypes.Cash:
                return "Cash";
                break;
            case paymentTypes.BankTransfer:
                return "Bank Transfer";
                break;
            case paymentTypes.Services:
                return "Services";
                break;
            case paymentTypes.Goods:
                return "Goods";
                break;
            default:
                return "Unknown";
                break;
        }
    };
    
    var clearFormData = function() {
        $("#add-debt-container fieldset input[type!='button']").val("");
    };

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
            var options = {
                relatveUrl: "~/debt/Add",
                httpMethod: "POST",
                dataPayload: debtData,
                progressContainerIdOrClassName: "add-debt-container",
                successCallback: function () {
                    getDebtSummaryHtml();
                    $.nyroModalRemove();
                },
                errorCallback: function () {
                    $("#add-debt-container fieldset").fadeIn();
                },
                errorMessage: "There was a problem adding the debt record to the system. Please try again.",
                typeOfError: window.payMeBack.notificationEngine.MESSAGE_TYPE_ERROR,
                ignoreResultStatus: false
            };
            window.payMeBack.ajaxManager.ajaxRequest(options);
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
            cache: false,
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

    var invokeCallback = function(callback) {
        if (typeof callback !== 'undefined') {
            callback();
        }
    };

    var populateEditDebtForm = function(paymentPlan, selectedDebtId) {
        var numTotalDebt = paymentPlan.DebtsOwedToMe.length;
        var currentDebt = null;
        for (var cnt = 0; cnt < numTotalDebt; cnt++) {
            currentDebt = paymentPlan.DebtsOwedToMe[cnt];
            if (currentDebt.Id === selectedDebtId) {
                break;
            }
        }

        if (currentDebt === null) {
            return;
        }

        var emailInput = $("#edit-debt-user-email");
        var debtAmountInput = $("#edit-debt-amount");
        var debtByDateInput = $("#edit-debt-end-date");
        var debtReasonText = $("#edit-debt-reason");
        var debtNotesText = $("#edit-debt-notes");

        if (currentDebt.InitialPayment !== 0) {
            $("#edit-debt-initial-payment").text(window.payMeBack.core.formatCurrency(currentDebt.InitialPayment));
        }

        emailInput.val(currentDebt.UserWhoOwesDebt.EmailAddress);
        emailInput.attr("title", currentDebt.UserWhoOwesDebt.FirstNames + " " + currentDebt.UserWhoOwesDebt.Surname);
        debtAmountInput.val(currentDebt.TotalAmountOwed);
        if (currentDebt.ExpectedEndDate && currentDebt.ExpectedEndDate !== null) {
            debtByDateInput.val(currentDebt.ExpectedEndDate);
        }
        debtReasonText.val(currentDebt.ReasonForDebt);
        debtNotesText.val(currentDebt.Notes);

        var tableRowArea = $("#edit-debt-payments-container table tbody");
        tableRowArea.empty();

        if (currentDebt.PaymentInstallments && currentDebt.PaymentInstallments.length > 0) {
            var length = currentDebt.PaymentInstallments.length;
            var rowTemplate = $("#installment-row-template");
            for (var rowCnt = 0; rowCnt < length; rowCnt++) {
                var installment = currentDebt.PaymentInstallments[rowCnt];
                var html = rowTemplate.html();
                var row = html.replace("{{date}}", window.payMeBack.core.formatDate(installment.PaymentDate,"ddd d/m/y"))
                            .replace("{{amount}}", window.payMeBack.core.formatCurrency(installment.AmountPaid))
                            .replace("{{type}}", getPaymentTypeDescrption(installment.TypeOfPayment));
                tableRowArea.append(row);
            }
        }
    };

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
            var options = {
                relatveUrl: "~/debt/AddPayment",
                httpMethod: "POST",
                dataPayload: payment,
                progressContainerIdOrClassName: "add-debt-payment-section",
                statusMsgContainerSelector: "#add-debt-payment-section",
                successCallback: function (result) {
                    $("#add-debt-payment-container").fadeOut('normal', function () {
                        invokeCallback(completionCallback);
                    });
                    getDebtSummaryHtml();
                },
                errorCallback: function () {
                    $("#add-debt-payment-container fieldset").fadeIn();
                },
                errorMessage: "There was a problem adding a payment",
                typeOfError: window.payMeBack.notificationEngine.MESSAGE_TYPE_ERROR,
                ignoreResultStatus: false
            };
            window.payMeBack.ajaxManager.ajaxRequest(options);
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
    };

    var deleteDebt = function (debtId) {
        var options = {
            relatveUrl: "~/debt/Delete?debtId=" + debtId,
            httpMethod: "DELETE",
            dataPayload: null,
            progressContainerIdOrClassName: null,
            statusMsgContainerSelector: null,
            successCallback: function (result) {
                $('#debt-summary-owed tr[data-debt-id="' + debtId + '"]').fadeOut('normal', function () {
                    $(this).remove();
                });
                if (result && result.total) {
                    $("#debts-owed-to-me span.total-owed-amount").text(result.total);
                }
            },
            errorCallback: _undefined,
            errorMessage: "An error has occurred",
            typeOfError: window.payMeBack.notificationEngine.MESSAGE_TYPE_ERROR,
            ignoreResultStatus: false
        };

        window.payMeBack.ajaxManager.ajaxRequest(options);
    };

    var getDebtDetails = function (debtId) {
        var options = {
            relatveUrl: "~/api/debts",
            progressContainerIdOrClassName: "edit-debt-container",
            successCallback: function (result) {
                $("#edit-debt-container fieldset").fadeIn();
                populateEditDebtForm(result, debtId);
            },
            errorCallback: function () {
                $("#edit-debt-container fieldset").fadeIn();
            },
            errorMessage: "There was a problem retrieving the debt details. Please try again.",
            typeOfError: window.payMeBack.notificationEngine.MESSAGE_TYPE_SMALL_ERROR,
            ignoreResultStatus: true
        };
        window.payMeBack.ajaxManager.ajaxRequest(options);

    }

    var editDebt = function (debtId, completionCallback) {
        $.nyroModalManual({
            url: '#edit-debt-modal',
            minHeight: 320,
            height: 470,
            minWwidth: 500,
            bgColor: window.payMeBack.core.colours.nyroModalBackground,
            endRemove: function () {
                var fieldForm = $("#edit-debt-container fieldset");
                $("input[type='text']", fieldForm).val("");
                $("input[type='date']", fieldForm).val("");
                $("input[type='number']", fieldForm).val("");
                $("textarea", fieldForm).val("");
                fieldForm.hide();
                $("#edit-debt-container .progress-indicator").hide();
                if (typeof completionCallback !== 'undefined') {
                    completionCallback();
                }

            },
            endShowContent: function () {
                var debtContainer = $("#edit-debt-container");
                $(".progress-indicator", debtContainer).hide();
                $("fieldset ul li input", debtContainer).unbind().on("keypress", function (e) {
                    if (e.which === 13) {
                        alert('not done');
                    }
                });
                $("#edit-debt-submit").unbind().on("click", function (e) {
                    alert('not done');
                });
                getDebtDetails(debtId);


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
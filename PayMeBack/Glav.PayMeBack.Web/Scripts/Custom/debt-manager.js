/// <reference path="_references.js" />
/// <reference path="ajaxManager.js" />
/// <reference path="notificationEngine.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.debtManager = {};
}

window.payMeBack.debtManager = (function () {

    var _defaultDateFormat = "d/m/y";

    var submitFormData = function (debtData, successCallback) {
        $("#add-debt-container fieldset").fadeOut('normal', function () {
            var options = {
                relatveUrl: "~/debt/Add",
                httpMethod: "POST",
                dataPayload: debtData,
                progressContainerIdOrClassName: "add-debt-container",
                successCallback: function () {
                    $.nyroModalRemove();
                    invokeCallback(successCallback);
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
                //$("fieldset ul li input", debtContainer).unbind().on("keypress", function (e) {
                //    if (e.which === 13) {
                //        captureEditDebtFormAndSubmit();
                //    }
                //});
                //$("#edit-debt-submit").unbind().on("click", function (e) {
                //    captureEditDebtFormAndSubmit();
                //});
                $("#edit-debt-container fieldset").removeClass("hidden").fadeIn();

                //window.payMeBack.inputManager.maskAllMoneyInputControls();
            }
        });
    };

    return {
        showAddDebtDialog: function (completionCallback) { showAddDebtDialog(completionCallback); },
        submitFormData: submitFormData,
        editDebt: function (debtId, completionCallback) { editDebt(debtId, completionCallback); },
        showAddPaymentToDebtDialog: function (debtId, xPos, yPos, completionCallback) { showAddPaymentToDebtDialog(debtId, xPos, yPos, completionCallback); }
    };
})();
/// <reference path="notification-manager.js" />
/// <reference path="_references.js" />

$(document).ready(function () {

    function bindGridActions() {
        $("#debt-summary-owed tr td.action-icon").on('click', function (evt) {
            // get id from data-debt-id attr on TR element which is parent
            var el = $(this);
            evt.preventDefault();
            evt.stopImmediatePropagation();
            var debtId = el.parents("tr").attr("data-debt-id");

            if (el.hasClass("action-delete")) {
                window.payMeBack.notificationEngine.showConfirmationContextMessage(null, "Deleting this debt will remove it entirely", function () {
                    window.payMeBack.debtManager.deleteDebt(debtId);
                });
            }
            if (el.hasClass("action-add-payment")) {
                // unbind existing table events otherwise they will interfer with the dialog
                // that is shown
                $("#debt-summary-owed tbody td").unbind();
                // Get the position of the clicked element and pass it to the showAddPayment method
                var xPos = evt.clientX;
                var yPos = evt.clientY + $(window).scrollTop();
                window.payMeBack.debtManager.showAddPaymentToDebtDialog(debtId, xPos, yPos, function () {
                    bindDebtRowSelectBehaviour();
                });
            }
        });
    }

    function bindActionLinks() {
        $("#add-debt-link").unbind().on("click", function () {
            window.payMeBack.debtManager.showAddDebtDialog(bindGridActions);
        });
        $("#set-notification-link").unbind().on("click", function () {
            window.payMeBack.notificationManager.showNotificationOptionsForDebt(bindGridActions);
        });
        $("#manage-profile-link").unbind().on("click", function () {
            alert('sorry, not complete');
        });
    }

    function bindDebtRowSelectBehaviour() {
        $("table.debt-summary-table tr.debt-item-row td").each(function () {
            var row = $(this);
            row.unbind().on('click', function () {
                var el = $(this);
                if (!el.hasClass('action-icon')) {
                    var debtId = el.parents("tr").attr("data-debt-id");
                    window.payMeBack.debtManager.editDebt(debtId);
                }
            });
        });
        bindActionLinks();
        bindGridActions();
    }

    bindDebtRowSelectBehaviour();
});

/// <reference path="_references.js" />

$(document).ready(function () {
    function bindActionLinks() {
        $("#add-debt-link").on("click", function () {
            window.payMeBack.debtManager.showAddDebtDialog();
        });
    }

    function bindGridActions() {
        $("table tr td.action-delete a").on('click', function (evt) {
            // get id from data-debt-id attr on TR element which is parent
            var el = $(this);
            evt.preventDefault();
            evt.stopImmediatePropagation();
            var debtId = el.parents("tr").attr("data-debt-id");
            window.payMeBack.notificationEngine.showConfirmationContextMessage(null, "Deleting this debt will remove it entirely", function () {
                window.payMeBack.debtManager.deleteDebt(debtId);
            });
        });
    }

    function bindDebtRowSelectBehaviour() {
        $("table.debt-summary-table tr.debt-item-row td").each(function () {
            var row = $(this);
            row.on('click', function () {
                var el = $(this);
                if (!el.hasClass('action-icon')) {
                    var debtId = el.parents("tr").attr("data-debt-id");
                    window.payMeBack.debtManager.editDebt(debtId);
                }
            });
        });
    }

    bindActionLinks();
    bindGridActions();
    bindDebtRowSelectBehaviour();
});

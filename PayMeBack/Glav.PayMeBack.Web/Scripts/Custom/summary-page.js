/// <reference path="_references.js" />

$(document).ready(function () {
    function bindActionLinks() {
        $("#add-debt-link").on("click", function () {
            window.payMeBack.debtManager.showAddDebtDialog();
        });
    }

    bindActionLinks();
});

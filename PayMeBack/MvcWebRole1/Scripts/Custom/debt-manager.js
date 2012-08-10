/// <reference path="_references.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.debtManager = {};
}

window.payMeBack.debtManager = (function () {

    var showAddDebtDialog = function () {
        $.nyroModalManual({
            url: '#add-debt-modal',
            minHeight: 300,
            height: 400,
            width: 500,
            bgColor: window.payMeBack.core.colours.nyroModalBackground,
            //modal: true,
            //closeButton: null,
            endRemove: function () {
            },
            endShowContent: function () {
            }
        });
    };

    return {
        showAddDebtDialog: function () { showAddDebtDialog(); }
    };
})();
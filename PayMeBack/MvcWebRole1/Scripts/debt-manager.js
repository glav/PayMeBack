﻿/// <reference path="_references.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.debtManager = {};
}

window.payMeBack.debtManager = (function () {

    var showAddDebtDialog = function () {
        $.nyroModalManual({
            url: '#add-debt-modal',
            minHeight: 300,
            height: 300,
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
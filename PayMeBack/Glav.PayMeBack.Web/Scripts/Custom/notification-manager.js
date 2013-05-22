/// <reference path="_references.js" />
/// <reference path="ajaxManager.js" />
/// <reference path="notificationEngine.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.notificationManager = {};
}

window.payMeBack.notificationManager = (function () {

    var showNotificationOptionsForDebt = function(competedCallback, onRemoved, onContentShown) {
        $.nyroModalManual({
            url: '#notification-options-modal',
            minHeight: 300,
            height: 400,
            minWwidth: 500,
            bgColor: window.payMeBack.core.colours.nyroModalBackground,
            //modal: true,
            //closeButton: null,
            endRemove: function () {
                if (typeof onRemoved !== 'undefined') {
                    onRemoved();
                }
                //$("#notification-options-container fieldset").show();
                //$("#notification-options-container .progress-indicator").hide();
                //if (typeof completionCallback !== 'undefined') {
                //    completionCallback();
                //}

            },
            endShowContent: function () {
                if (typeof onContentShown !== 'undefined') {
                    onContentShown();
                }
                //var container = $("#notification-options-container");
                //$(".progress-indicator", container).hide();
            }
        });
    };

    return {
        showNotificationOptionsForDebt: showNotificationOptionsForDebt
    };
})();
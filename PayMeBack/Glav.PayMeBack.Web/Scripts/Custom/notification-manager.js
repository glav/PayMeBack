/// <reference path="_references.js" />
/// <reference path="ajaxManager.js" />
/// <reference path="notificationEngine.js" />

if (typeof window.payMeBack.debtManager === 'undefined') {
    window.payMeBack.notificationManager = {};
}

window.payMeBack.notificationManager = (function () {

    var showNotificationOptionsForDebt = function(competedCallback) {
        alert('getting there, but this section is not complete');
    };
    
    return {
        showNotificationOptionsForDebt: showNotificationOptionsForDebt
    };
})();
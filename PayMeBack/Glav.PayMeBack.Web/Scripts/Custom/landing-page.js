/// <reference path="_references.js" />

$(document).ready(function () {
    window.payMeBack.login.updateDisplayBasedOnSignedInStatus();
    if (window.payMeBack.login.isUserSignedIn() === true) {
        location.assign(window.payMeBack.core.makePathFromVirtual("~/summary"));
    }
});

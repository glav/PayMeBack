﻿/* File Created: August 1, 2012 */

$(document).ready(function() {

    window.payMeBack.login.showLoginDialog(true, false, function () {
        location.assign(window.payMeBack.core.makePathFromVirtual("~/summary"));
    });

});
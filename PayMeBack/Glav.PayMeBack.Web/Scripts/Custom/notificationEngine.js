/// <reference path="_references.js" />

if (typeof window.payMeBack.notificationEngine === 'undefined') {
    window.payMeBack.notificationEngine = {};
}

window.payMeBack.notificationEngine = (function () {

    var DELAY_IN_SECONDS = 5;  // how long the status bar remains visible before auto-retracting

    var MESSAGE_TYPE_INFO = 1;
    var MESSAGE_TYPE_ERROR = 2;

    var showPopupDialogMessage = function (message) {
        //TODO: make it a nyroModal or something sexier
        alert(message);
    };


    // This will show a simple message that slides down, then disappears after a bit
    // sorta like Stack overflow
    // If no container element is specified, we assume the entire browser/main window
    // is the container
    var showStatusBarMessage = function (message, messageType, containerSelector, topPositionOffset) {
        var typeOfMessage = MESSAGE_TYPE_INFO;
        if (typeof messageType !== 'undefined') {
            if (messageType === "error" || messageType === MESSAGE_TYPE_ERROR) {
                typeOfMessage = MESSAGE_TYPE_ERROR;
            }
        }

        var messageDivClass;
        if (typeOfMessage === MESSAGE_TYPE_ERROR) {
            messageDivClass = "error";
        } else {
            messageDivClass = "info";
        }
        var htmlBlock = "<div class='status-message hidden'><div class='" + messageDivClass + "'>" + message + "</div></div>";
        var messageArea = null;
        if (typeof containerSelector !== 'undefined') {
            var containerEl = $(containerSelector);
            if (containerEl.length > 0) {
                messageArea = containerEl;
            }
        }
        if (messageArea === null) {
            messageArea = $("body");
        }

        var htmlEl = $(htmlBlock);
        messageArea.append(htmlEl);
        if (typeof topPositionOffset !== 'undefined') {
            htmlEl.css('top', topPositionOffset + 'px');
        }
        htmlEl.slideDown('normal', function () {
            setTimeout(function () {
                htmlEl.slideUp('fast', function () {
                  htmlEl.remove();
                });
            }, DELAY_IN_SECONDS * 1000);
        });
    };

    return {
        showPopupDialogMessage: function (message) { showPopupDialogMessage(message); },
        showStatusBarMessage: function (message, containerEl, topPositionOffset) { showStatusBarMessage(message, containerEl, topPositionOffset); },
        MessageTypeInfo: MESSAGE_TYPE_INFO,
        MessageTypeError: MESSAGE_TYPE_ERROR
    };
})();
/// <reference path="_references.js" />

if (typeof window.payMeBack.notificationEngine === 'undefined') {
    window.payMeBack.notificationEngine = {};
}

window.payMeBack.notificationEngine = (function () {

    var DELAY_IN_SECONDS = 5;  // how long the status bar remains visible before auto-retracting

    var MESSAGE_TYPE_INFO = 1;
    var MESSAGE_TYPE_ERROR = 2;
    var MESSAGE_TYPE_SMALL_ERROR = 3;

    var showPopupDialogMessage = function (message) {
        //TODO: make it a nyroModal or something sexier
        alert(message);
    };

    // This function will present a smallnon intrusive confirmation action to the user
    // but not modally and typically attached to an element for context. If the user
    // cofirms the action then the confirmation callback is executed
    var showConfirmationContextMessage = function (triggerElement, message, confirmCallback) {
        var htmlBlock = "<div id='status-message' class='status-message confirm-container hidden'><div class='confirm'>" + message + " - Are you sure?<span class='confirm-action'><a href='#yes'>Yes</a><a href='#no'>No</a></span></div></div>";

        if (typeof triggerElement === 'undefined' || triggerElement === '' || triggerElement === null) {
            triggerElement = $("body");
        }
        var htmlEl = $(htmlBlock);
        triggerElement.append(htmlEl);
        $("a", htmlEl).click('on', function () {
            var aEl = $(this);
            var triggerCallback = false;
            if (aEl.attr('href') === "#yes") {
                triggerCallback = true;
            }
            htmlEl.slideUp('normal', function () {
                htmlEl.remove();
                if (triggerCallback === true && typeof confirmCallback !== 'undefined') {
                    confirmCallback();
                }
            });

        });

        //$("body").on("click", function () {
        //    var msgEl = $("#status-message");
        //    msgEl.slideUp('normal', function () {
        //        msgEl.remove();
        //    });
        //});
        $(window).scrollTop(0);
        htmlEl.slideDown('normal');

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
            } else if (messageType === MESSAGE_TYPE_SMALL_ERROR) {
                typeOfMessage = MESSAGE_TYPE_SMALL_ERROR;
            }
        }

        var messageDivClass;
        if (typeOfMessage === MESSAGE_TYPE_ERROR) {
            messageDivClass = "error";
        } else if (typeOfMessage === MESSAGE_TYPE_SMALL_ERROR) {
            messageDivClass = "small-error";
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
        showStatusBarMessage: function (message, messageType, containerEl, topPositionOffset) { showStatusBarMessage(message, messageType, containerEl, topPositionOffset); },
        showConfirmationContextMessage: function (triggerElement, message, confirmationCallback) { showConfirmationContextMessage(triggerElement, message, confirmationCallback); },
        MessageTypeInfo: MESSAGE_TYPE_INFO,
        MessageTypeError: MESSAGE_TYPE_ERROR,
        MessageTypeSmallError: MESSAGE_TYPE_SMALL_ERROR
    };
})();
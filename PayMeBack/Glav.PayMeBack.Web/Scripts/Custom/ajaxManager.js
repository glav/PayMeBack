/// <reference path="_references.js" />

if (typeof window.payMeBack.ajaxManager === 'undefined') {
    window.payMeBack.ajaxManager = {};
}

window.payMeBack.ajaxManager = (function () {

    var ajaxRequest = function (relativeUrl, httpMethod, dataPayload, progressContainerIdOrClassName, statusMsgContainerSelector, successCallback, errorCallback) {
        window.payMeBack.progressManager.showProgressIndicator(progressContainerIdOrClassName);
        $.ajax({
            url: window.payMeBack.core.makePathFromVirtual(relativeUrl),
            type: httpMethod,
            data: JSON.stringify(dataPayload),
            contentType: 'application/json',
            dataType: "json",
            cache: false,
            success: function (result) {
                window.payMeBack.progressManager.hideProgressIndicator(progressContainerIdOrClassName, function () {
                    if (result && typeof result.error !== 'undefined') {
                        window.payMeBack.notificationEngine.showStatusBarMessage("The request had an error: " + result.error, window.payMeBack.notificationEngine.MessageTypeError, statusMsgContainerSelector, 5);
                        if (typeof errorCallback !== 'undefined') {
                            errorCallback(result);
                        }

                    } else {
                        if (result && typeof result.success !== 'undefined' && result.success === true) {
                            if (typeof successCallback !== 'undefined') {
                                successCallback(result);
                            }
                        } else {
                            var msg = "";
                            if (typeof result.error !== 'undefined') {
                                msg = result.error;
                            }

                            msg = "Sorry, there was an error performing your request. " + msg;
                            window.payMeBack.notificationEngine.showStatusBarMessage(msg, window.payMeBack.notificationEngine.MessageTypeError, statusMsgContainerSelector);
                            if (typeof errorCallback !== 'undefined') {
                                errorCallback(result);
                            }
                        }
                    }
                });
            },
            error: function (e) {
                window.payMeBack.progressManager.hideProgressIndicator(progressContainerIdOrClassName, function () {
                    var msg = "There was a problem adding the debt record to the system. Please try again.";
                    window.payMeBack.notificationEngine.showStatusBarMessage(msg, window.payMeBack.notificationEngine.MessageTypeError, statusMsgContainerSelector, 5);
                    if (typeof errorCallback !== 'undefined') {
                        errorCallback(result);
                    }
                });
            }
        });
    }

    return {
        ajaxRequest: function (relativeUrl, httpMethod, dataPayload, progressContainerIdOrClassName, statusMsgContainerSelector, successCallback, errorCallback) { ajaxRequest(relativeUrl, httpMethod, dataPayload, progressContainerIdOrClassName, statusMsgContainerSelector, successCallback, errorCallback); }
    };

})();
/// <reference path="_references.js" />

if (typeof window.payMeBack.ajaxManager === 'undefined') {
    window.payMeBack.ajaxManager = {};
}

window.payMeBack.ajaxManager = (function () {

    var ajaxRequest = function (relativeUrl, httpMethod, dataPayload, progressContainerIdOrClassName, statusMsgContainerSelector, successCallback, errorCallback, errorMessage, typeOfError) {
        window.payMeBack.progressManager.showProgressIndicator(progressContainerIdOrClassName);
        var genericErrorMessage = "There was a problem performing your request.";
        if (typeof errorMessage !== 'undefined') {
            genericErrorMessage = errorMessage;
        }
        var errorType = window.payMeBack.notificationEngine.MessageTypeError;
        if (typeof typeOfError !== 'undefined') {
            errorType = typeOfError;
        }
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
                        window.payMeBack.notificationEngine.showStatusBarMessage("The request had an error: " + result.error, errorType, statusMsgContainerSelector, 5);
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
                            window.payMeBack.notificationEngine.showStatusBarMessage(msg, errorType, statusMsgContainerSelector);
                            if (typeof errorCallback !== 'undefined') {
                                errorCallback(result);
                            }
                        }
                    }
                });
            },
            error: function (e) {
                window.payMeBack.progressManager.hideProgressIndicator(progressContainerIdOrClassName, function (e) {
                    window.payMeBack.notificationEngine.showStatusBarMessage(genericErrorMessage, errorType, statusMsgContainerSelector, 5);
                    if (typeof errorCallback !== 'undefined') {
                        if (typeof result !== 'undefined') {
                            errorCallback(result);
                        } else {
                            errorCallback();
                        }
                    }
                });
            }
        });
    }

    return {
        ajaxRequest: function (relativeUrl, httpMethod, dataPayload, progressContainerIdOrClassName, statusMsgContainerSelector, successCallback, errorCallback, errorMessage, typeOfError) { ajaxRequest(relativeUrl, httpMethod, dataPayload, progressContainerIdOrClassName, statusMsgContainerSelector, successCallback, errorCallback, errorMessage, typeOfError); }
    };

})();
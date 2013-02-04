/// <reference path="_references.js" />
/// <reference path="notificationEngine.js" />

if (typeof window.payMeBack.ajaxManager === 'undefined') {
    window.payMeBack.ajaxManager = {};
}

window.payMeBack.ajaxManager = (function () {
    var _undefined;

    var _defaultOptions = {
        relatveUrl: "",
        httpMethod: "GET",
        dataPayload: null,
        progressContainerIdOrClassName: "",
        statusMsgContainerSelector: "#nyroModalContent",
        successCallback: _undefined,
        errorCallback: _undefined,
        errorMessage: "An error has occurred",
        typeOfError: window.payMeBack.notificationEngine.MessageTypeError,
        ignoreResultStatus: false
    };

    // Sets up common ajax parameters such as OAuth accesstokens
    var ajaxSetup = function (options) {
        var opts = $.extend(_defaultOptions, options);
        opts.headers = { "Authorization": "Bearer " + window.payMeBack.auth.accessToken };
        opts.cache = false;
        $.ajaxSetup(opts);
    };

    var fireCallback = function (callback, arg) {
        if (typeof callback !== 'undefined') {
            if (typeof arg !== 'undefined') {
                callback(arg);
            } else {
                callback();
            }
        }
    };

    var isRequestResultSuccessful = function (result) {
        if (typeof result !== "undefined") {
            if (typeof result.error !== "undefined" || (typeof result.IsSuccessful !== "undefined" && result.IsSuccessful === false)) {
                return false;
            }
        }
        return true;
    };

    /// <summary>
    /// Makes an Ajax request using standard parameters and hides/displays the progress indicator.Also
    /// will display a message if error or information typeof messge using std notification mechanism
    /// </summary>
    //var ajaxRequest = function (relativeUrl, httpMethod, dataPayload, progressContainerIdOrClassName, statusMsgContainerSelector, successCallback, errorCallback, errorMessage, typeOfError, ignoreResultStatus) {
    var ajaxRequest = function (options) {
        window.payMeBack.progressManager.showProgressIndicator(options.progressContainerIdOrClassName);

        ajaxSetup(options);
        $.ajax({
            url: window.payMeBack.core.makePathFromVirtual(options.relatveUrl),
            type: options.httpMethod,
            data: JSON.stringify(options.dataPayload),
            contentType: 'application/json',
            dataType: "json",
            cache: false,
            success: function (result) {
                window.payMeBack.progressManager.hideProgressIndicator(options.progressContainerIdOrClassName, function () {
                    // If we are asked to ignore the result status, it means we are probably
                    // making a pure API call and so result.status will not be presented, thus we 
                    // check if the option has been passed in and ignore the statis if requred.
                    if (options.ignoreResultStatus === true) {
                        fireCallback(options.successCallback, result);
                        return;
                    }

                    if (isRequestResultSuccessful(result)) {
                        fireCallback(options.successCallback, result);
                    } else {
                        var msg = "";
                        if (typeof result.error !== 'undefined') {
                            msg = result.error;
                        } else if (typeof result.ErrorMessages !== "undedined") {
                            var errLength = result.ErrorMessages.length;
                            for (var cnt=0; cnt < errLength; cnt++) {
                                if (msg !== "") {
                                    msg += ", ";
                                }
                                msg += result.ErrorMessages[cnt];
                            }
                        }

                        msg = "Sorry, there was an error performing your request. " + msg;
                        window.payMeBack.notificationEngine.showStatusBarMessage(msg, options.typeOfError, options.statusMsgContainerSelector);
                        fireCallback(options.errorCallback, result);
                    }
                });
            },
            error: function (e) {
                window.payMeBack.progressManager.hideProgressIndicator(options.progressContainerIdOrClassName, function (e) {
                    window.payMeBack.notificationEngine.showStatusBarMessage(options.errorMessage, options.typeOfError, options.statusMsgContainerSelector, 5);
                    fireCallback(options.errorCallback, result);
                });
            }
        });
    };

    return {
        ajaxRequest: function (options) { ajaxRequest(options); },
        ajaxSetup: function (options) { ajaxSetup(options); },
        isRequestResultSuccessful: function(requestResult) { return isRequestResultSuccessful(requestResult);}
    };

})();
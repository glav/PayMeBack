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
    var ajaxSetup = function () {
        var opts = $.ajaxSettings;
        opts.headers = { "Authorization": "Bearer " + window.payMeBack.auth.accessToken };
        opts.cache = false;
        $.ajaxSetup(opts);
    }

    var fireCallback = function (callback, arg) {
        if (typeof callback !== 'undefined') {
            if (typeof arg !== 'undefined') {
                callback(arg);
            } else {
                callback();
            }
        }
    };

    /// <summary>
    /// Makes an Ajax request using standard parameters and hides/displays the progress indicator.Also
    /// will display a message if error or information typeof messge using std notification mechanism
    /// </summary>
    //var ajaxRequest = function (relativeUrl, httpMethod, dataPayload, progressContainerIdOrClassName, statusMsgContainerSelector, successCallback, errorCallback, errorMessage, typeOfError, ignoreResultStatus) {
        var ajaxRequest = function (options) {
            var opts = $.extend(_defaultOptions, options);
        window.payMeBack.progressManager.showProgressIndicator(opts.progressContainerIdOrClassName);

        ajaxSetup();
        $.ajax({
            url: window.payMeBack.core.makePathFromVirtual(opts.relatveUrl),
            type: opts.httpMethod,
            data: JSON.stringify(opts.dataPayload),
            contentType: 'application/json',
            dataType: "json",
            cache: false,
            success: function (result) {
                window.payMeBack.progressManager.hideProgressIndicator(opts.progressContainerIdOrClassName, function () {
                    if (result && typeof result.error !== 'undefined') {
                        window.payMeBack.notificationEngine.showStatusBarMessage("The request had an error: " + result.error, opts.typeOfError, opts.statusMsgContainerSelector, 5);
                        fireCallback(opts.errorCallback,result);
                    } else {
                        // If we are asked to ignore the result status, it means we are probably
                        // making a pure API call and so result.status will not be presented, thus we 
                        // check if the option has been passed in and ignore the statis if requred.
                        if (opts.ignoreResultStatus === true) {
                            fireCallback(opts.successCallback,result);
                            return;
                        }

                        if (result && typeof result.success !== 'undefined' && result.success === true) {
                                fireCallback(opts.successCallback, result);
                        } else {
                            var msg = "";
                            if (typeof result.error !== 'undefined') {
                                msg = result.error;
                            }

                            msg = "Sorry, there was an error performing your request. " + msg;
                            window.payMeBack.notificationEngine.showStatusBarMessage(msg, opts.typeOfError, opts.statusMsgContainerSelector);
                                fireCallback(opts.errorCallback,result);
                        }
                    }
                });
            },
            error: function (e) {
                window.payMeBack.progressManager.hideProgressIndicator(opts.progressContainerIdOrClassName, function (e) {
                    window.payMeBack.notificationEngine.showStatusBarMessage(opts.errorMessage, opts.typeOfError, opts.statusMsgContainerSelector, 5);
                    fireCallback(opts.errorCallback, result);
                });
            }
        });
    }

    return {
        ajaxRequest: function (options) { ajaxRequest(options); },
        ajaxSetup: function () { ajaxSetup(); }
    };

})();
/// <reference path="_references.js" />

if (typeof window.payMeBack.progressManager === 'undefined') {
    window.payMeBack.progressManager = {};
}

window.payMeBack.progressManager = (function () {
    var idOrClassName = "progress-indicator";

    var findProgressElements = function (containerElementIdOrClass) {
        var container = null;
        if (typeof containerElementIdOrClass !== 'undefined') {
            var container = $("#" + containerElementIdOrClass);
            if (container.length === 0) {
                container = $("." + containerElementIdOrClass);
            }
        }
        var progressElements = null;
        if (container !== null && container.length > 0) {
            progressElements = $("#" + idOrClassName, container);
            if (progressElements.length === 0) {
                progressElements = $("." + idOrClassName, container);
            }
        }

        if (progressElements === null || progressElements.length === 0) {
            progressElements = $("#" + idOrClassName);
            if (progressElements.length === 0) {
                progressElements = $("." + idOrClassName);
            }
        }

        return progressElements;
    };

    var fireCompletionCallback = function (completionCallback) {
        if (typeof completionCallback !== 'undefined') {
            completionCallback();
        }
    };

    var hideProgressIndicator = function (containerElement, completionCallback) {
        /// This function will try and hide the element first with id matching idOrClassName var
        /// then if it does not find it, try and display the element with that className
        /// It will use a container element as the root of the search if it is provided

        var progressElements = findProgressElements(containerElement, completionCallback);
        if (progressElements.length > 0) {
            progressElements.fadeOut('normal', function () { fireCompletionCallback(completionCallback); });
        }
    };

    var showProgressIndicator = function (containerElement, completionCallback) {
        /// This function will try and display the element first with id matching idOrClassName var
        /// then if it does not find it, try and display the element with that className
        /// It will use a container element as the root of the search if it is provided

        var progressElements = findProgressElements(containerElement);
        if (progressElements.length > 0) {
            progressElements.fadeIn('normal', function () { fireCompletionCallback(completionCallback); });
        }
    };


    return {
        showProgressIndicator: function (containerElement, completionCallback) { showProgressIndicator(containerElement, completionCallback); },
        hideProgressIndicator: function (containerElement, completionCallback) { hideProgressIndicator(containerElement, completionCallback); }
    };

})();
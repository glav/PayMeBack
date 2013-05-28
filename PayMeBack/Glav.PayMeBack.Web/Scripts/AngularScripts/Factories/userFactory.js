/// <reference path="../_references.js" />

window.payMeBack.app.factory(window.payMeBack.core.dependencies.userFactory, [function () {

    return {
        isUserSignedIn: function () {
            var state = $("#is-signed-in-state").val();
            return (typeof state !== 'undefined' && state === "true");
        }
    };

    
}]);
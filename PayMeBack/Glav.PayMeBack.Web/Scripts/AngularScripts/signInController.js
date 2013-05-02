/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.signInController, function ($scope, userFactory) {

    $scope.isUserSignedIn = userFactory.isUserSignedIn;
    $scope.signIn = function () {
        // convert window.paymeback.login to a service
        window.payMeBack.login.showLoginDialog(false, false);
    };
    $scope.signUp = function () {
        // convert window.paymeback.login to a service
        window.payMeBack.login.showLoginDialog(false, true);
    };

});

/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.signInController, function ($scope, userFactory) {

    $scope.isUserSignedIn = userFactory.isUserSignedIn;
    $scope.signIn = function () {
        window.payMeBack.login.showLoginDialog(false, false);
    };
    $scope.signUp = function () {
        window.payMeBack.login.showLoginDialog(false, true);
    };

});

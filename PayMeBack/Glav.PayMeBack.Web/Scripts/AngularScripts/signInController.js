/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.signInController, function ($scope, $rootScope, userFactory) {

    $scope.isUserSignedIn = userFactory.isUserSignedIn;
    $scope.signIn = function () {
        $rootScope.isSignupAction = false;
        // convert window.paymeback.login to a service
        window.payMeBack.login.showLoginDialog(false, false);
    };
    $scope.signUp = function () {
        $rootScope.isSignupAction = true;
        // convert window.paymeback.login to a service
        window.payMeBack.login.showLoginDialog(false, true);
    };

});

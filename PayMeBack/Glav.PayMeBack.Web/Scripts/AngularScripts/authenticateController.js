/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.authenticateController, function ($scope) {

    init();

    $scope.submitCredentials = function () {
        window.payMeBack.login.submitCredentials($scope.loginData, $scope.isSignupAction, function () {
            location.assign(window.payMeBack.core.makePathFromVirtual("~/summary"));
        });
    };

    function init() {
        $scope.loginData = {
            email: '',
            password: '',
            firstname: '',
            surname: ''
        }
        $scope.isSignupAction = false;
    }
});

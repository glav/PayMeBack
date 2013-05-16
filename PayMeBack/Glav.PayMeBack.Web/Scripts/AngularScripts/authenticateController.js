/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.authenticateController, function ($scope, $rootScope) {

    init();

    $scope.submitCredentials = function () {
        window.payMeBack.login.submitCredentials($scope.loginData, $scope.isSignupAction, function () {
            $rootScope.isSignupAction = false;
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
    }
});

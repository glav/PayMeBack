/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.authenticateController, ['$scope', '$rootScope','$timeout',
    function ($scope, $rootScope, $timeout) {

    init();

    $scope.submitCredentials = function () {
        if ($scope.loginForm.$invalid) {
            return;
        }
        window.payMeBack.login.submitCredentials($scope.loginData, $scope.isSignupAction, function () {
            $rootScope.isSignupAction = false;
            //Note: once we convert the login to not use the nyroModal, then we can remove
            // this timeout rubbish
            $timeout(function () {
                //small delay while the result is returned.This causes apply to be called
                // and angular will trigger some binding magic and display an update
                $timeout(function () {
                    // now  we do a redirect
                    location.assign(window.payMeBack.core.makePathFromVirtual("~/summary"));
                }, 2000);
            }, 500);

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
    }]);

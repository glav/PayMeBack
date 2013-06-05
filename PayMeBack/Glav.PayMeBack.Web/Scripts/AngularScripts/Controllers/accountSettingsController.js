/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.accountSettingsController,
    function ($scope, debtFactory, eventFactory, userFactory, messageService, errorService) {

        init();

        function init() {
            $scope.isInProgress = true;
            userFactory.getUserDetails().then(function (result) {
                $scope.isInProgress = false;
                $scope.userData = result;
            });

        }

        $scope.isPasswordEntered = function () {
            
            if ($scope.userData && $scope.userData.NewPassword && $scope.userData.NewPassword !== ''
                    && $scope.userData.NewPassword !== null) {
                return true;
            }
            return false;
        }

        $scope.submitAcctSettingsForm = function () {
            if ($scope.acctSettings.$invalid) {
                return;
            }

            userFactory.updateUserDetails($scope.userData)
                    .then(function (result) {
                        if (!errorService.isSuccess(result)) {
                            var msg = messageService.constructErrorMessage(result, "Sorry, wecould not update your details.");
                            window.payMeBack.notificationEngine.showStatusBarMessage(msg,window.payMeBack.notificationEngine.MessageTypeError, "#account-settings-container");
                        } else {
                            eventFactory.triggerCloseAllDialogs();
                        }
                    });
        };

        $scope.opts = {
            backdropFade: true,
            dialogFade: true
        };


    }).$inject = ['$scope', 'debtFactory', 'eventFactory', 'userFactory', 'messageService', 'errorService'];

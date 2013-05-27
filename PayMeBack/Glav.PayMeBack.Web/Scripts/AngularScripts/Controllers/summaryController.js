/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.summaryController,
    function ($scope, $rootScope, debtFactory,$dialog) {

        refreshSummaryList();

        function refreshSummaryList() {
            $scope.IsInProgress = true;

            // Retrieve the paymentplan details and place into rootscope for things
            // like the edit controller
            debtFactory.getPaymentPlan().then(function (result) {
                $rootScope.paymentPlan = result;
            });

            // Get the debt summary items
            $scope.debts = debtFactory.getDebtSummaryItems()
                .then(function (result) {
                    $scope.IsInProgress = false;
                    return result;
            });

        }

        $scope.$on('debtSummaryListChanged', refreshSummaryList);
        $scope.$on('closeAllDialogs', function () {
            refreshSummaryList();
            $scope.closeEditDebtModal();
        });

        $scope.addPayment = function (id, $event) {
            var xPos = $event.clientX;
            var yPos = $event.clientY;
            $rootScope.debtId = id;

            window.payMeBack.debtManager.showAddPaymentToDebtDialog(xPos, yPos, id);
            $event.stopPropagation();
        };

        $scope.deleteDebt = function (id, $event) {
            $event.stopPropagation();
            window.payMeBack.notificationEngine.showConfirmationContextMessage(null, "Deleting this debt will remove it entirely", function () {
                debtFactory.deleteDebt(id)
                    .then(function () {
                        //refreshSummaryList();
                        debtFactory.triggerRefresh();
                    });
            });
        };
        
        $scope.editDebt = function (id) {
            $rootScope.debtId = id;
            $scope.editDebtModal = true;
            //debtFactory.triggerActiveItemChanged(id);
            //window.payMeBack.debtManager.editDebt(id);
        };

        $scope.closeEditDebtModal = function () {
            $scope.editDebtModal = false;
        };

        $scope.editNotification = function (id, $event) {
            debtFactory.triggerActiveItemChanged(id);
            $scope.editNotificationModal = true;
            $event.stopPropagation();

           
            //window.payMeBack.notificationManager.showNotificationOptionsForDebt(undefined, function () {
            //    $rootScope.notifyProgress = false;
            //    //$("#notification-options-container fieldset").show();
            //    //$("#notification-options-container .progress-indicator").hide();
            //}, function () {
            //    $rootScope.notifyProgress = false;
            //    $scope.$digest();
            //    //var container = $("#notification-options-container");
            //    //$(".progress-indicator", container).hide();
            //});
        };

        $scope.closeEditNotificationModal = function () {
            $scope.editNotificationModal = false;
        }

        $scope.opts = {
            backdropFade: true,
            dialogFade: true
        };
    }
);

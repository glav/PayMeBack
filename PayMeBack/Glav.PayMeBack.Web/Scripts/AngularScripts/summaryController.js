/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.summaryController,
    function ($scope, $rootScope, debtFactory) {

        refreshSummaryList();

        function refreshSummaryList() {
            $scope.IsInProgress = true;

            $scope.debts = debtFactory.getDebtSummaryItems()
                .then(function (result) {
                    $scope.IsInProgress = false;
                    return result;
            });
            
        }

        $scope.$on('debtSummaryListChanged', refreshSummaryList);

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
            debtFactory.triggerActiveItemChanged(id);
            window.payMeBack.debtManager.editDebt(id);
        };

        $scope.editNotification = function (id, $event) {
            $event.stopPropagation();
            debtFactory.triggerActiveItemChanged(id);
            window.payMeBack.notificationManager.showNotificationOptionsForDebt();
        };
    }
);

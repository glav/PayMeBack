﻿/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.summaryController,
    function ($scope, $rootScope, debtFactory) {

        refreshSummaryList();

        function refreshSummaryList() {
            $scope.debts = debtFactory.getDebtSummaryItems();
        }

        $scope.$on('debtSummaryListChanged', refreshSummaryList);

        $scope.addPayment = function (id, $event) {
            var xPos = $event.client;
            var yPos = $event.clientY;
            $rootScope.debtId = id;

            window.payMeBack.debtManager.showAddPaymentToDebtDialog(xPos, yPos, id);
            $event.stopPropagation();
        };

        $scope.deleteDebt = function (id) {
            window.payMeBack.notificationEngine.showConfirmationContextMessage(null, "Deleting this debt will remove it entirely", function () {
                debtFactory.deleteDebt(id)
                    .then(function () {
                        refreshSummaryList();
                        debtFactory.triggerRefresh();
                    });
            });
        };
        
        $scope.editDebt = function (id) {
            $rootScope.debtId = id;
            debtFactory.triggerActiveItemChanged(id);
            window.payMeBack.debtManager.editDebt(id);
        };
    }
);

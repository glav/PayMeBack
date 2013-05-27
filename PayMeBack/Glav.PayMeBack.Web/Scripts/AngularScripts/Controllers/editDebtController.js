/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.editDebtController,
    function ($scope, $rootScope, debtFactory, eventFactory) {

        //$scope.debt = {};

        init();

        function init() {
            if ($scope.debtId !== undefined) {
                getCurrentDebtFromPlan($scope.debtId);
            }
        }

        function getCurrentDebtFromPlan(id) {
            $scope.debt = {};
            var numTotalDebt = $scope.paymentPlan.DebtsOwedToMe.length;
            var currentDebt = null;
            for (var cnt = 0; cnt < numTotalDebt; cnt++) {
                currentDebt = $scope.paymentPlan.DebtsOwedToMe[cnt];
                if (currentDebt.Id === id) {
                    $scope.debt = currentDebt;
                }
            }
        };

        $scope.getPaymentDescription = debtFactory.getPaymentTypeDescrption;

        $scope.submitEditForm = function () {
            // If form is invalid, then simply return and leave modal open
            if ($scope.editdebt.$invalid) {
                return;
            }
            var numTotalDebt = $scope.paymentPlan.DebtsOwedToMe.length;
            var currentDebt = $scope.debt;
            for (var cnt = 0; cnt < numTotalDebt; cnt++) {
                if ($scope.paymentPlan.DebtsOwedToMe[cnt].Id === currentDebt.Id) {
                    $scope.paymentPlan.DebtsOwedToMe[cnt] = currentDebt;
                    break;
                }
            }
            debtFactory.updatePaymentPlan($scope.paymentPlan).then(function (result) {
                $("#edit-debt-payments-container").data("plan", result);
                eventFactory.triggerRefresh();
                eventFactory.triggerCloseAllDialogs();
            });

        };
    }
);

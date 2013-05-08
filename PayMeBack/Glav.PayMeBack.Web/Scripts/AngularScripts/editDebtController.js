/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.editDebtController,
    function ($scope, debtFactory) {

        $scope.debt = {};
        refreshPlanFromSource();

        function refreshPlanFromSource() {
            debtFactory.getPaymentPlan().then(function (result) {
                $scope.paymentPlan = result;
                $("#edit-debt-payments-container").data("plan", result);
                return result;
            });

        }

        $scope.$on('debtSummaryListChanged', refreshPlanFromSource);

        $scope.$on('debtActiveItemChanged', function (event, id) {
            // inefficient. See how we can make the code commented out work
            //$scope.debt = debtFactory.getDebt(id);

            var numTotalDebt = $scope.paymentPlan.DebtsOwedToMe.length;
            var currentDebt = null;
            for (var cnt = 0; cnt < numTotalDebt; cnt++) {
                currentDebt = $scope.paymentPlan.DebtsOwedToMe[cnt];
                if (currentDebt.Id === id) {
                    $scope.debt = currentDebt;
                }
            }

        });

        $scope.getPaymentDescription = debtFactory.getPaymentTypeDescrption;

        $scope.submitEditForm = function () {
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
                debtFactory.triggerRefresh();
                $.nyroModalRemove();
            });
            
        };
        //debtFactory.triggerRefresh();
    }
);

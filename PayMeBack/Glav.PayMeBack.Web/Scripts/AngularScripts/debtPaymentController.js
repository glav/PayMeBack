/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.debtPaymentController,
    function ($scope, $rootScope, debtFactory) {

    init();

    function init() {
        $scope.paymentData = {
            DebtId: '',  // from $rootScope
            AmountPaid: 0,
            PaymentDate: window.payMeBack.core.formatDate(null, "y-m-d"),
            TypeOfPayment: 1
        };

        $scope.paymentTypeOptions = [
            { value: 1, label: 'Cash' },
            { value: 2, label: 'Bank Transfer' },
            { value: 3, label: 'Services' },
            { value: 4, label: 'Goods' }
        ];
    }

    $scope.submitDebtDataToServer = function () {
        $scope.paymentData.DebtId = $scope.debtId;

        debtFactory.addPayment($scope.paymentData)
            .then(function () {
                $("#add-debt-payment-container").fadeOut();
                $rootScope.$broadcast('debtSummaryListChanged');
                /* broadvast refresh event*/
            });
    }
});

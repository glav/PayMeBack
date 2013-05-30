/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.debtPaymentController,
    function ($scope, debtFactory,dateFilter,eventFactory) {

    init();

    function init() {
        $scope.paymentData = {
            DebtId: '',  // from $rootScope
            AmountPaid: 0,
            PaymentDate: dateFilter(new Date(), "dd-MM-yyyy"),
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
        if ($scope.addPaymentForm.$invalid) {
            return;
        }
        $scope.paymentData.DebtId = $scope.debtId;
        debtFactory.addPayment($scope.paymentData)
            .then(function () {
                $("#add-debt-payment-container").fadeOut();
                eventFactory.triggerRefresh();
            });
    }
    }).$inject = ['$scope', 'debtFactory', 'dateFilter','eventFactory'];

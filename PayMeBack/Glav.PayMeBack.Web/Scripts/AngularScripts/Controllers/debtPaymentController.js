/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.debtPaymentController,
    function ($scope, debtFactory, dateFilter, eventFactory, dateService) {

        init();


        function clearData() {
            $scope.paymentData = {
                DebtId: '',  // from $rootScope
                AmountPaid: 0,
                PaymentDate: dateFilter(new Date(), "dd-MM-yyyy"),
                TypeOfPayment: 1
            };
        }

        function init() {
            clearData();

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

            var result = dateService.isDateValue($scope.paymentData.PaymentDate, 'dd-MM-yyyy');
            if (result.isValid) {
                $scope.paymentData.PaymentDate = dateFilter(result.dateData, 'yyyy-MM-dd');
            } else {
                return;
            }

            $scope.paymentData.DebtId = $scope.debtId;
            debtFactory.addPayment($scope.paymentData)
                .then(function () {
                    $("#add-debt-payment-container").fadeOut();
                    eventFactory.triggerRefresh();
                    clearData();
                });
        }
    }).$inject = ['$scope', 'debtFactory', 'dateFilter', 'eventFactory', 'dateService'];

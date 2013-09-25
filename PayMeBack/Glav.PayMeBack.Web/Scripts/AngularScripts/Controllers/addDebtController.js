/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.addDebtController,['$scope', '$rootScope', 'debtFactory', 'eventFactory','dateService',
    function ($scope, $rootScope, debtFactory, eventFactory, dateService) {

    init();

    function init() {
        $scope.debtData = {
            emailAddress: '',
            amountOwed: null,
            debtReason: '',
            initialAmountPaid: 0,
            notes: '',
            paymentPeriod: 0,
            expectedEndDate: ''
        };

        $scope.paymentPeriodOptions = [
            { value: 0, label: 'Ad-hoc' },
            { value: 1, label: 'Daily' },
            { value: 2, label: 'Weekly' },
            { value: 3, label: 'Fortnightly' },
            { value: 4, label: 'Monthly' },
            { value: 5, label: 'Bi-Monthly' },
            { value: 6, label: 'Quarterly' },
            { value: 7, label: 'Half Yearly' },
            { value: 8, label: 'Yearly' }
        ];

        $scope.addDebtInProgress = false;
    }

    $scope.submitDebtDataToServer = function () {
        if ($scope.addDebtForm.$invalid) {
            return;
        }
        $scope.addDebtInProgress = true;

        if ($scope.debtData.expectedEndDate !== '') {
            var result = dateService.isDateValue($scope.debtData.expectedEndDate, 'dd-MM-yyyy');
            if (result.isValid) {
                $scope.debtData.expectedEndDate = dateFilter(result.dateData, 'yyyy-MM-dd');
            } else {
                return;
            }
        }

        debtFactory.addDebt($scope.debtData).then(function () {
            eventFactory.triggerRefresh();
            eventFactory.triggerCloseAllDialogs();
            init();
        });
    }
    }]);

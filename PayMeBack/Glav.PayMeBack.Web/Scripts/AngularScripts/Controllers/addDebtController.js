/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.addDebtController,
    function ($scope, $rootScope, debtFactory, eventFactory) {

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
        debtFactory.addDebt($scope.debtData).then(function () {
            eventFactory.triggerRefresh();
            eventFactory.triggerCloseAllDialogs();
            init();
        });
    }
    }).$inject = ['$scope', '$rootScope', 'debtFactory', 'eventFactory'];

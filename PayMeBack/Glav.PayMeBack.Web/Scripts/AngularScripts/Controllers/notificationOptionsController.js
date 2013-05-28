/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.notificationOptionsController,
    function ($scope, notificationFactory) {

    init();

    $scope.$on('debtActiveItemChanged', function (event, id) {
        $scope.notifyOptions = notificationFactory.getNotificationOptions(id);
    });

    $scope.submitOptionsForm = function () {
        $scope.notifyProgress = true;
        alert('not done yet');
        $scope.notifyProgress = false;
    };


    function init() {
        $scope.inProgress = false;
        $scope.notificationOptions = [
            { value: 0, label: 'None' },
            { value: 1, label: 'Email' },
            { value: 2, label: 'Sms' }
        ];
        $scope.notificationInterval = [
            { value: 0, label: 'Unspecified' },
            { value: 1, label: 'Daily' },
            { value: 2, label: 'Weekly' },
            { value: 3, label: 'Monthly' },
            { value: 4, label: 'Yearly' }
        ];
        $scope.weekDays = [
            { value: 0, label: 'Monday' },
            { value: 1, label: 'Tuesday' },
            { value: 2, label: 'Wednessday' },
            { value: 3, label: 'Thursday' },
            { value: 4, label: 'Friday' },
            { value: 5, label: 'Saturday' },
            { value: 6, label: 'Sunday' }
        ];
    }
    }).$inject = ['$scope', 'notificationFactory'];

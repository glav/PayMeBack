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

        $scope.isEmailSelected = function () {
            return ($scope.notifyOptions.NotificationMethod === 1);
        };
        $scope.isSmsSelected = function () {
            return ($scope.notifyOptions.NotificationMethod === 2);
        };
        $scope.isSelectionMade = function () {
            return ($scope.notifyOptions.NotificationMethod > 0);
        };
        $scope.isReminderIntervalSelectionMade = function () {
            return ($scope.notifyOptions.Interval.Frequency > 0);
        };
        $scope.isIntervalWeekly = function () {
            return ($scope.notifyOptions.Interval.Frequency === 2);
        };

        function init() {
            $scope.inProgress = false;
            $scope.notifyOptions = {
                NotificationMethod: 0,
                Interval: {
                    Frequency: 0,
                    FrequencyCount: 0,
                    WeekDay: 0,
                    Time: 0
                }
            };
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

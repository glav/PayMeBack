/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.accountSettingsController,
    function ($scope, debtFactory, eventFactory) {

    $scope.submitAcctSettingsForm = function () {
        eventFactory.triggerCloseAllDialogs();
    };

});

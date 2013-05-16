/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.summaryActionLinkController, function ($scope) {

    // For the add debt link
    $scope.showAddDebtDialog = function () {
        window.payMeBack.debtManager.showAddDebtDialog();
    };

    $scope.showManageProfileDialog = function () {
        window.payMeBack.accountSettingsManager.showAccountSettingsForUser();
    };
});

/// <reference path="../_references.js" />

window.payMeBack.app.config(function ($locationProvider, $routeProvider) {
    $locationProvider.html5Mode(true);
    $routeProvider.when('/summary/AddDebt',
        {
            //templateUrl: "#add-debt-modal",
            controller: window.payMeBack.core.dependencies.addDebtController
        });
}
);

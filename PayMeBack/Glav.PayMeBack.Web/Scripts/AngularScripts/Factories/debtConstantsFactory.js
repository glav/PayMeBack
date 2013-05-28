/// <reference path="../_references.js" />

window.payMeBack.app.factory(window.payMeBack.core.dependencies.debtConstantsFactory, [function () {
    return {
        paymentTypes: {
            Unknown: 0,
            Cash: 1,
            BankTransfer: 2,
            Services: 3,
            Goods: 4
        }
    }
}]);
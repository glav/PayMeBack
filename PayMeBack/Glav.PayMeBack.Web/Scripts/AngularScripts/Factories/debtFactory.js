/// <reference path="../_references.js" />

window.payMeBack.app.factory(window.payMeBack.core.dependencies.debtFactory, ['$http','$rootScope','debtConstantsFactory',function ($http, $rootScope, debtConstantsFactory) {

    return {
        addPayment: function (payment) {
            return $http(
                {
                    method: "PUT",
                    url: "/api/payment",
                    data: payment,
                    headers: { "Authorization": "Bearer " + window.payMeBack.auth.accessToken }
                }).then(function (result) {
                    return result.data;
                });

        },
        deleteDebt: function (id) {
            return $http(
                {
                    method: "DELETE",
                    url: "/api/debts?debtId=" + id,
                    headers: { "Authorization": "Bearer " + window.payMeBack.auth.accessToken }
                }).then(function (result) {
                    return result.data;
                });
        },

        getPaymentPlan: function() {
            return $http(
                {
                    method: "GET",
                    url: "/api/debts",
                    headers: { "Authorization": "Bearer " + window.payMeBack.auth.accessToken }
                }).then(function (result) {
                    if (result.status === 200) {
                        return result.data;
                    }

                    return {};
                });
        },

        updatePaymentPlan: function(paymentPlan) {
            return $http(
                {
                    method: "POST",
                    url: "/api/debts",
                    data: paymentPlan,
                    headers: { "Authorization": "Bearer " + window.payMeBack.auth.accessToken }
                }).then(function (result) {
                    if (result.status === 200) {
                        return result.data;
                    }

                    return {};
                });
        },

        getDebt: function (id) {
            if (typeof id === 'undefined') {
                return {};
            }
            return $http(
                {
                    method: "GET",
                    url: "/api/debts",
                    headers: { "Authorization": "Bearer " + window.payMeBack.auth.accessToken }
                }).then(function (result) {
                    if (result.status === 200) {
                        var numTotalDebt = result.data.DebtsOwedToMe.length;
                        var currentDebt = null;
                        for (var cnt = 0; cnt < numTotalDebt; cnt++) {
                            currentDebt = result.data.DebtsOwedToMe[cnt];
                            if (currentDebt.Id === id) {
                                return currentDebt;
                            }
                        }
                    }

                    return {};
                });
        },
        addDebt: function(debt) {
            if (typeof debt === 'undefined') {
                return {};
            }
            return $http(
                {
                    method: "POST",
                    url: "/debt/Add",
                    data:debt,
                    headers: { "Authorization": "Bearer " + window.payMeBack.auth.accessToken }
                }).then(function (result) {
                    if (result.status === 200) {
                        return result.data;
                    }

                    return {};
                });

        },

        getDebtSummaryItems: function () {
            // return a promise via angular JS
            return $http(
                {
                    method: "GET",
                    url: "/api/summary",
                    headers: { "Authorization": "Bearer " + window.payMeBack.auth.accessToken }
                }).then(function (result) {
                    return result.data;
                });

        },

        getPaymentTypeDescrption: function (paymentTypeValue) {
            switch (paymentTypeValue) {
                case debtConstantsFactory.paymentTypes.Unknown:
                    return "Unknown";
                    break;
                case debtConstantsFactory.paymentTypes.Cash:
                    return "Cash";
                    break;
                case debtConstantsFactory.paymentTypes.BankTransfer:
                    return "Bank Transfer";
                    break;
                case debtConstantsFactory.paymentTypes.Services:
                    return "Services";
                    break;
                case debtConstantsFactory.paymentTypes.Goods:
                    return "Goods";
                    break;
                default:
                    return "Unknown";
                    break;
            }
        }

    };


}]);
/// <reference path="../_references.js" />

window.payMeBack.app.factory(window.payMeBack.core.dependencies.debtFactory, function ($http) {

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
                    url: "/api/debts?debtId="+id,
                    headers: { "Authorization": "Bearer " + window.payMeBack.auth.accessToken }
                }).then(function (result) {
                    return result.data;
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

            //var options = {
            //    relatveUrl: "~/api/summary",
            //    httpMethod: "GET",
            //    dataPayload: null,
            //    progressContainerIdOrClassName: null,
            //    statusMsgContainerSelector: null,
            //    successCallback: function (result) {
            //        //$scope.debts = result; // how do we return this?
            //        $rootScope.$broadcast("debtSummaryChanged", result);
            //    },
            //    errorMessage: "An error has occurred",
            //    typeOfError: window.payMeBack.notificationEngine.MESSAGE_TYPE_ERROR,
            //    ignoreResultStatus: false
            //};

            //window.payMeBack.ajaxManager.ajaxRequest(options);
        }
    };


});
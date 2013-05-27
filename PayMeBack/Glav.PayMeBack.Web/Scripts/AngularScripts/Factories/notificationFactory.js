/// <reference path="../_references.js" />

window.payMeBack.app.factory(window.payMeBack.core.dependencies.notificationFactory, function ($http) {

    return {
        getNotificationOptions: function (debtId) {
            return $http(
            {
                method: "GET",
                url: "/api/notification/"+debtId,
                headers: { "Authorization": "Bearer " + window.payMeBack.auth.accessToken }
            }).then(function (result) {
                return result.data;
            });

        }

    };


});
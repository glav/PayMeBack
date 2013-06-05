/// <reference path="../_references.js" />

window.payMeBack.app.factory(window.payMeBack.core.dependencies.userFactory, ['$http', function ($http) {

    return {
        isUserSignedIn: function () {
            var state = $("#is-signed-in-state").val();
            return (typeof state !== 'undefined' && state === "true");
        },

        getUserDetails: function () {
            return $http(
                {
                    method: "GET",
                    url: "/user",
                    headers: { "Authorization": "Bearer " + window.payMeBack.auth.accessToken }
                }).then(function (result) {
                    return result.data;
                });

        },
        updateUserDetails: function (userDetails) {
            return $http(
                {
                    method: "POST",
                    url: "/user/Update",
                    data:userDetails,
                    headers: { "Authorization": "Bearer " + window.payMeBack.auth.accessToken }
                }).then(function (result) {
                    return result.data;
                }, function (e) {
                    return { success: false, status: e.status };
                });

        }

    };


}]);
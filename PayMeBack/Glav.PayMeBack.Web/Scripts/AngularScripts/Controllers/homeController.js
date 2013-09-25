/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.homeController, ['$scope', 'userFactory',
    function ($scope, userFactory) {

        $scope.isUserSignedIn = userFactory.isUserSignedIn;

        if ($scope.isUserSignedIn() === true) {
            location.assign(window.payMeBack.core.makePathFromVirtual("~/summary"));
        }
    }]);

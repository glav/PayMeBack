/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.homeController,
    function ($scope, userFactory) {

    $scope.isUserSignedIn = userFactory.isUserSignedIn;

    if ($scope.isUserSignedIn() === true) {
        location.assign(window.payMeBack.core.makePathFromVirtual("~/summary"));
    }
    }).$inject = ['$scope', 'userFactory'];

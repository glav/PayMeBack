/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.signInController, function ($scope, userFactory) {

    $scope.isUserSignedIn = userFactory.isUserSignedIn;

});

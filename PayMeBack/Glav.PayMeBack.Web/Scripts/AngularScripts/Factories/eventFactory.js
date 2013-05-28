/// <reference path="../_references.js" />

window.payMeBack.app.factory(window.payMeBack.core.dependencies.eventFactory, ['$rootScope',function ($rootScope) {

    return {
        triggerRefresh: function () {
            $rootScope.$broadcast('debtSummaryListChanged');
        },
        triggerActiveItemChanged: function (id) {
            $rootScope.$broadcast('debtActiveItemChanged', id);
        },
        triggerCloseAllDialogs: function () {
            $rootScope.$broadcast('closeAllDialogs');
        },
    };

    
}]);
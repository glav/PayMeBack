
window.payMeBack.app.directive('addPayment', ['$rootScope',function ($rootScope) {
    return {
        restrict: 'A', // A=attribute, E = element, C = class, M =comment
        link: function (scope, element, attrs, ctrl) {
            
            element.on("click", function (e) {
                var debtId = attrs.addPayment;
                var xPos = e.clientX;
                var yPos = e.clientY;
                $rootScope.debtId = debtId;

                window.payMeBack.debtManager.showAddPaymentToDebtDialog(xPos, yPos, debtId);
                e.stopPropagation();
                //if (e.which === 13) {
                //    $('#'+id).click();
                //}
            });
        }
    };
}]);





window.payMeBack.app.directive('addPayment', ['$rootScope',function ($rootScope) {
    return {
        restrict: 'A', // A=attribute, E = element, C = class, M =comment
        link: function (scope, element, attrs, ctrl) {
            
            element.on("click", function (e) {
                var debtId = attrs.addPayment;
                var xPos = e.clientX;
                var yPos = e.clientY;
                $rootScope.debtId = debtId;

                var container = $("#add-debt-payment-container");
                $("fieldset", container).show();
                container.css('left', xPos + 'px')
                    .css('top', yPos + 'px')
                    .fadeIn('normal', function () {
                        // If the user clicks outside the dialog on the body somewhere, the close the dialog
                        $("#add-debt-payment-close").on('click', function (e) {
                            container.fadeOut('normal', function () {
                                $(this).fadeOut();
                            });
                        });

                        $("#payment-amount").focus();
                    });
                e.stopPropagation();
                //if (e.which === 13) {
                //    $('#'+id).click();
                //}
            });
        }
    };
}]);




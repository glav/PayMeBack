
window.payMeBack.app.directive('defaultButton', function () {
    return {
        restrict: 'AE', // A=attribute, E = element, C = class, M =comment
        link: function (scope, element, attrs) {
            var id = attrs.defaultButton || attrs.button;
            $('input[type!="button"]', element).on("keypress", function (e) {
                if (e.which === 13) {
                    $('#'+id).click();
                }
            });
        }
    };
});




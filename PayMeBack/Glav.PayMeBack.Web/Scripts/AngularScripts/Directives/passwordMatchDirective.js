
window.payMeBack.app.directive('passwordMatch', [
    function () {
        return {
            require: 'ngModel',// without this,you dont get the controller (see ctrl below)
            // you also need a ng-model directive on the element before you
            // get the controller instance as a parameter
            restrict: 'A', // A=attribute, E = element, C = class, M =comment
            link: function (scope, elm, attrs, ctrl) {

                var compareControlSelector = attrs.passwordMatch;

                function getCompareControlValue() {
                    var compareControlValue = undefined;
                    var control = $("#" + compareControlSelector);
                    if (control.length === 0) {
                        control = $("input[name='" + compareControlSelector + "']");
                    }
                    if (control.length === 1) {
                        compareControlValue = control.val();
                    }

                    return compareControlValue;
                }

                function doControlValuesMatch(compareValue) {
                    if (compareValue === undefined) {
                        ctrl.$setValidity('passwordMatch', true);
                        return compareValue;
                    }

                    if (compareValue === getCompareControlValue() && compareValue !== undefined) {
                        ctrl.$setValidity('passwordMatch', true);
                        return compareValue;
                    }
                    ctrl.$setValidity('passwordMatch', false);
                    return undefined;

                }

                // Model to View update
                ctrl.$formatters.unshift(function (modelValue) {
                    return doControlValuesMatch(modelValue);
                });

                //View to Model update
                ctrl.$parsers.unshift(function (viewValue) {
                    return doControlValuesMatch(viewValue);
                });

            }
        };
    }]);




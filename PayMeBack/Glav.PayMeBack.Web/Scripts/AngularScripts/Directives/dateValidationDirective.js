/// <reference path="../Services/dateService.js" />

window.payMeBack.app.directive('dateValid', ['dateFilter', 'dateService',
    function (dateFilter, dateService) {
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {

                var dateFormat = attrs.dateValid || "yyyy-MM-dd";
                //var dateFormat = 'd-m-y';


                // Model to View update
                ctrl.$formatters.unshift(function (modelValue) {
                    var result;
                    if (dateService.isDateInSerialisedIsoFormat(modelValue)) {
                        // When getting data from the model/server, it always gets
                        // serialised as yyyy-MM-dd
                        result = dateService.isDateValue(modelValue, 'yyyy-MM-dd');
                    } else {
                        result = dateService.isDateValue(modelValue, dateFormat);
                    }

                    if (!result.isValid) {
                        ctrl.$setValidity('dateValid', false);
                        return undefined;
                    }
                    ctrl.$setValidity('dateValid', true);
                    return dateService.formattedDate(result.dateData, dateFormat);
                });

                //NOTE: This does not seem to set the field to invalid or anything like that
                //      Need to investigate why
                //View to Model update
                ctrl.$parsers.unshift(function (viewValue) {
                    var result = dateService.isDateValue(viewValue, dateFormat);
                    if (result.isValid) {
                        ctrl.$setValidity('dateValid', true);
                        //return result.dateData;
                        return formattedDate(result.dateData);
                    }
                    ctrl.$setValidity('dateValid', false);
                    return undefined;
                });
            }
        };
    }]);





window.payMeBack.app.directive('dateValid', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, elm, attrs, ctrl) {
            var dateFormat = attrs.moMediumDate;
            var dateFormat = 'd-m-y';

            function isDateValue(dateValue) {
                var testDate = new Date(dateValue);
                var isValid = testDate.getFullYear() > 1950 && isNaN(testDate.getFullYear()) !== true;
                return {
                    isValid: isValid,
                    dateData: testDate
                };
            }

            // Model to View update
            ctrl.$formatters.unshift(function (modelValue) {
                console.log('modelValue=' + modelValue);
                if (!dateFormat || !modelValue) {
                    ctrl.$setValidity('dateValid', false);
                    return undefined;
                }

                var result = isDateValue(modelValue);
                if (!result.isValid) {
                    ctrl.$setValidity('dateValid', false);
                    return undefined;
                }
                var retVal = window.payMeBack.core.formatDate(result.dateData, dateFormat);
                ctrl.$setValidity('dateValid', true);
                return retVal;
            });

            //View to Model update
            ctrl.$parsers.unshift(function (viewValue) {
                //debugger;
                ctrl.$setValidity('dateValid', false);
                var result = isDateValue(viewValue);
                console.log('result.isValid=' + result.isValid +  "result.dateData=" + result.dateData);
                if (result.isValid) {
                    ctrl.$setValidity('dateValid', true);
                    return result.dateData.toDateString();
                }
                ctrl.$setValidity('dateValid', false);
                return undefined;
                //var date = moment(viewValue, dateFormat);
                //return (date && date.isValid() && date.year() > 1950) ? date.toDate() : "";
            });
        }
    };
});




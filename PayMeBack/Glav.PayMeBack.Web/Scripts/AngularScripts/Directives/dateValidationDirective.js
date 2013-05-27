
window.payMeBack.app.directive('dateValid', function (dateFilter) {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, elm, attrs, ctrl) {

            var dateFormat = attrs.dateValid || "yyyy-mm-dd";
            //var dateFormat = 'd-m-y';

            function isDateValue(dateValue) {
                var dateComponents = getDateComponents(dateValue);
                var testDate = createDateFromComponents(dateComponents, dateFormat);
                //var testDate = new Date(dateValue);
                var isValid = testDate.getFullYear() > 1950
                                && isNaN(testDate.getFullYear()) !== true
                                && isNaN(testDate.getMonth()) !== true
                                && isNaN(testDate.getDate()) !== true;
                return {
                    isValid: isValid,
                    dateData: testDate
                };
            }

            // utilise separators
            function getDateComponents(dateValue) {
                // COnvert any supported separators (- and .) into '/'
                dateValue = dateValue.replace(/-/g, '/');
                //dateValue = dateValue.replace(/./g, '/');
                var components = dateValue.split('/');
                return components;

            }

            function createDateFromComponents(components, format) {
                if (components.length !== 3) {
                    return new Date(0, 0, 0);
                }

                var normalisedFormat = format.replace(/-/g, '/');
                //normalisedFormat = normalisedFormat.replace(/./g, '/');
                var normalisedFormatComponents = normalisedFormat.split('/');
                var formatCount = normalisedFormatComponents.length;
                var year, month, day;

                if (normalisedFormatComponents[0].toLowerCase().indexOf('y') >= 0) {
                    year = components[0];
                    if (formatCount > 0 && normalisedFormatComponents[1].toLowerCase().indexOf('m') >= 0) {
                        month = components[1];
                        if (formatCount > 1) {
                            day = components[2];
                        }
                    } else {
                        day = components[1];
                        if (formatCount > 1) {
                            month = components[2];
                        }
                    }

                } else {
                    year = components[2];
                    if (normalisedFormatComponents[0].toLowerCase().indexOf('m') >= 0) {
                        month = components[0];
                        if (formatCount > 0) {
                            day = components[1];
                        }
                    } else {
                        day = components[0];
                        if (formatCount > 1) {
                            month = components[1];
                        }
                    }
                }
                var intMonth = parseInt(month, 10);
                if (isNaN(intMonth) !== true && month <= 12 && month > 0) {
                    month = intMonth - 1;
                } else {
                    return new Date(0, 0, 0);
                }

                var intDay = parseInt(day, 10);
                if (isNaN(intDay) == true || intDay > 31 || day <= 0) {
                    return new Date(0, 0, 0);
                }
                var testDate = new Date(year, month, day);
                return testDate;
            }

            function formattedDate(dateValue) {
                return dateFilter(dateValue, dateFormat);
            }

            // Model to View update
            ctrl.$formatters.unshift(function (modelValue) {
                if (!dateFormat || !modelValue) {
                    ctrl.$setValidity('dateValid', false);
                    return undefined;
                }

                var result = isDateValue(modelValue);
                if (!result.isValid) {
                    ctrl.$setValidity('dateValid', false);
                    return undefined;
                }

                ctrl.$setValidity('dateValid', true);
                return formattedDate(modelValue);
            });

            //NOTE: This does not seem to set the field to invalid or anything like that
            //      Need to investigate why
            //View to Model update
            ctrl.$parsers.unshift(function (viewValue) {
                console.log(viewValue);
                var result = isDateValue(viewValue);
                if (result.isValid) {
                    ctrl.$setValidity('dateValid', true);
                    console.log('valid');
                    return formattedDate(result.dateData);
                }
                console.log('invalid');
                ctrl.$setValidity('dateValid', false);
                return undefined;
            });
        }
    };
});




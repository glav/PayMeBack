
window.payMeBack.app.service('dateService', ['dateFilter', function (dateFilter) {
    return {
        defaultDateFormat: "yyyy-MM-dd",

        isDateValue: function (dateValue, dateFormatToUse) {
            if (dateFormatToUse === undefined) {
                dateFormatToUse = this.defaultDateFormat;
            }
            var isValid = false;
            var testDate = null;

            if (dateValue === null || dateValue === "") {
                // Allow blank dates
                isValid = true;
            } else {
                //dateValue = this.stripTimeComponent(dateValue);
                var components = this.getComponents(dateValue);
                console.log(components);
                testDate = this.createDateFromComponents(components, dateFormatToUse);
                //var testDate = new Date(dateValue);
                isValid = testDate.getFullYear() > 1950
                                && isNaN(testDate.getFullYear()) !== true
                                && isNaN(testDate.getMonth()) !== true
                                && isNaN(testDate.getDate()) !== true;
            }
            return {
                isValid: isValid,
                dateData: testDate
            };
        },

        // utilise separators
        getComponents: function (dateValue) {
            if (dateValue) {
                var datePart, timePart;
                var tPos = dateValue.indexOf('T');
                if (tPos >= 0) {
                    datePart = dateValue.substr(0, tPos);
                    timePart = dateValue.substr(tPos + 1, dateValue.length - 1);
                } else {
                    datePart = dateValue;
                    timePart = "0:0:0";
                }
                // COnvert any supported separators (- and .) into '/'
                datePart = datePart.replace(/-/g, '/');
                //dateValue = dateValue.replace(/./g, '/');
                var dateComponents = datePart.split('/');
                var timeComponents = timePart.split(':');
                if (timeComponents.length >= 3) {
                    var dotPos = timeComponents[2].indexOf('.');
                    if (dotPos >= 0) {
                        timeComponents[2] = timeComponents[2].substr(0, dotPos);
                    }
                }

                return {
                    dateParts: dateComponents,
                    timeParts: timeComponents
                }
            }
            return {
                dateParts: [],
                timeParts: []
            };

        },

        createDateFromComponents: function (components, format) {
            if (components.dateParts.length !== 3) {
                return new Date(0, 0, 0);
            }

            var normalisedFormat = format.replace(/-/g, '/');
            var normalisedFormatComponents = normalisedFormat.split('/');
            var formatCount = normalisedFormatComponents.length;
            var year, month, day;

            if (normalisedFormatComponents[0].toLowerCase().indexOf('y') >= 0) {
                year = components.dateParts[0];
                if (formatCount > 0 && normalisedFormatComponents[1].toLowerCase().indexOf('m') >= 0) {
                    month = components.dateParts[1];
                    if (formatCount > 1) {
                        day = components.dateParts[2];
                    }
                } else {
                    day = components.dateParts[1];
                    if (formatCount > 1) {
                        month = components.dateParts[2];
                    }
                }

            } else {
                year = components.dateParts[2];
                if (normalisedFormatComponents[0].toLowerCase().indexOf('m') >= 0) {
                    month = components.dateParts[0];
                    if (formatCount > 0) {
                        day = components.dateParts[1];
                    }
                } else {
                    day = components.dateParts[0];
                    if (formatCount > 1) {
                        month = components.dateParts[1];
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
            console.log('year=' + year + ' month=' + month + ', day=' + day + ',hour=' + parseInt(components.timeParts[0], 10) + ', minute=' + parseInt(components.timeParts[1], 10));
            var testDate = new Date(year, month, day, parseInt(components.timeParts[0], 10), parseInt(components.timeParts[1], 10));
            return testDate;
        },

        formattedDate: function (dateValue, dateFormat) {
            if (dateFormat === undefined) {
                dateFormat = this.defaultDateFormat;
            }
            if (dateValue != null && dateValue != "") {
                return dateFilter(dateValue, dateFormat);
            } else {
                return "";
            }
        },

        isDateInSerialisedIsoFormat: function (dateValue) {
            return (dateValue && dateValue.indexOf && dateValue.indexOf('T') >= 0);
        }


    }

}]);

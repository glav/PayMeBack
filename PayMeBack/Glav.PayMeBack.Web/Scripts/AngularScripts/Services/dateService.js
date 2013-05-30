
window.payMeBack.app.service('dateService', ['dateFilter',function (dateFilter) {
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

                dateValue = this.stripTimeComponent(dateValue);
                var dateComponents = this.getDateComponents(dateValue);
                testDate = this.createDateFromComponents(dateComponents, dateFormatToUse);
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
        getDateComponents: function (dateValue) {
            // COnvert any supported separators (- and .) into '/'
            dateValue = dateValue.replace(/-/g, '/');
            //dateValue = dateValue.replace(/./g, '/');
            var components = dateValue.split('/');
            return components;

        },

        stripTimeComponent: function (dateValue) {
            if (dateValue.indexOf) {
                var pos = dateValue.indexOf('T');
                if (pos >= 0) {
                    var strippedDate = dateValue.substr(0, pos);
                    return strippedDate;
                }
            }
            return dateValue;
        },

        createDateFromComponents: function (components, format) {
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
            var currentDate = new Date();
            var testDate = new Date(year, month, day, currentDate.getHours(), currentDate.getMinutes());
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

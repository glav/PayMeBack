/* File Created: July 31, 2012 */

if (typeof window.payMeBack === 'undefined') {
	window.payMeBack = { };
}
if (typeof window.payMeBack.core === 'undefined') {

    window.payMeBack.core = {
        dependencies: {
            moduleName: "paymeback",
            userFactory: "userFactory",
            homeController: "homeController",
            summaryController: "summaryController",
            signInController: "signInController"
        },
        rootPath: "/",
        makePathFromVirtual: function(virtualPath) {
            if (typeof virtualPath === 'undefined') {
                return;
            }
            if (virtualPath.indexOf("~") !== 0) {
                return virtualPath;
            }

            var actualPath = virtualPath.replace("~",window.payMeBack.core.rootPath).replace("//","/");
            return actualPath;
        },
        
        formatDate: function (textDate, format) {
            var createDateString = function (dateObj, cultureFormat) {
                var days = ["Sunday", "Monday", "Tuesday", "Wednessday", "Thursday", "Friday", "Saturday"];
                var shortDayDesc = days[dateObj.getDay()].substr(0, 3);
                var day = dateObj.getDate();
                if (day < 10) {
                    day = "0" + day;
                }
                var month = dateObj.getMonth() + 1;
                if (month <10) {
                    month = "0" + month;
                }
                var year = dateObj.getFullYear();

                var formattedDate = cultureFormat.replace("ddd", shortDayDesc)
                    .replace("d", day)
                    .replace("m", month)
                    .replace("y", year);
                return formattedDate;
            };
            try {
                var dateFormat = "d/m/y";
                if (typeof format !== 'undefined') {
                    dateFormat = format;
                }

                // default to todays date if no initial date passed in
                var d;
                if (typeof textDate === "undefined" || textDate === null || textDate === "") {
                    d = new Date();
                } else {
                    d = new Date(textDate);
                }

                
                return createDateString(d, dateFormat);
            } catch (e) {
                return createDateString(new Date(), "d/m/y");
            }
        },
        
        formatCurrency: function(amount) {
            try {
                var amt = parseFloat(amount);
                var text = "$" + amt;
                if (text.indexOf(".") === -1) {
                    text += ".00";
                }
                return text;
            } catch (e) {
                return "$0.00";
            }
        }
    };
}

if (typeof window.payMeBack.core.colours === 'undefined') {
	window.payMeBack.core.colours = { };
}

/** Colour constants used in dynamic invocation and application ***/
window.payMeBack.core.colours.nyroModalBackground = "#A8A5A5";
window.payMeBack.core.colours.statusMessageInfoBackground = "#A8A5A5";
window.payMeBack.core.colours.statusMessageErrorBackground = "#A8A5A5";
/******************************************************************/

/***************** Authentcation and authorisation *********************/
if (typeof window.payMeBack.auth === 'undefined') {
    window.payMeBack.auth = {};
}

window.payMeBack.auth.accessToken = "";

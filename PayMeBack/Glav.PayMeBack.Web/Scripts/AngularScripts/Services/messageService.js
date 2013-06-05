
window.payMeBack.app.service('messageService', [function () {
    this.constructErrorMessage = function (errorData, optionalPrefixText) {

        if (!errorData) {
            return "Sorry, an error occurred.";
        }

        var msg;
        if (optionalPrefixText !== undefined) {
            msg = optionalPrefixText;
        } else {
            msg = "";
        }
        if (errorData.status) {
            var httpStatus = parseInt(errorData.status, 10);
            if (isNaN(httpStatus)) {
                httpStatus = 500;
            }

            if (httpStatus >= 400 && httpStatus < 500) {
                msg += " Error accessing the resource.";
            }
            if (httpStatus >= 500) {
                msg += " Server error processing request.";
            }

            msg += " [Code: " + httpStatus + "]";
        }

        return msg;
    };

}]);

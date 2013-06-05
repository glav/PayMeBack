
window.payMeBack.app.service('errorService', [function () {
    this.isSuccess = function (resultData) {
        return (resultData && resultData.success 
                    && (resultData.success === true || resultData.success.toString().toLowerCase() === 'true'));
    };

}]);

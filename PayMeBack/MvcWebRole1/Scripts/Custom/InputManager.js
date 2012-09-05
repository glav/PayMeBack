/// <reference path="_references.js" />

if (typeof window.payMeBack.inputManager === 'undefined') {
    window.payMeBack.inputManager = {};
}

window.payMeBack.inputManager = (function () {

    var allowMonetaryInputOnly = function (control) {
        $(control).on('keypress', function (e) {
            if (e.which === 9 || e.which === 8 || e.keyCode === 9 || e.keyCode === 8) {
                return;
            }
            var key = String.fromCharCode(e.which);
            if ("1234567890-$,.".indexOf(key) < 0) {
                e.cancel = true;
                e.stopImmediatePropagation();
                e.preventDefault();
                return false;
            }
        });
    };

    var maskAllMoneyInputControls = function () {
        $("input.input-money").each(function () {
            allowMonetaryInputOnly(this);
        });
    };

    return {
        allowMonetaryInputOnly: function (control) { allowMonetaryInputOnly(control); },
        maskAllMoneyInputControls: function () { maskAllMoneyInputControls(); }
    };
})();

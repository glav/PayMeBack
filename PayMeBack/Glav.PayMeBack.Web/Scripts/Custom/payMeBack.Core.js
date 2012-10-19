/* File Created: July 31, 2012 */

if (typeof window.payMeBack === 'undefined') {
	window.payMeBack = { };
}
if (typeof window.payMeBack.core === 'undefined') {

	window.payMeBack.core = {
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

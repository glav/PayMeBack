/* File Created: July 31, 2012 */

$(document).ready(function() {
   function bindLoginSignUpAction(){
   	$("div.header-menu li a.credentials-link").unbind().bind("click", function (e) {
   		var isSignUp = $(e.target).hasClass("sign-up");
          window.payMeBack.login.showLoginDialog(false,isSignUp);
       });
   }

    bindLoginSignUpAction();
    window.payMeBack.login.updateDisplayBasedOnSignedInStatus();
});
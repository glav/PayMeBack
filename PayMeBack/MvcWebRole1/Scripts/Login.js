/**
 * Created with JetBrains WebStorm.
 * User: Glav
 * Date: 31/07/12
 * Time: 5:31 PM
 * To change this template use File | Settings | File Templates.
 */

if (typeof window.payMeBack.login === 'undefined') {
    window.payMeBack.login = {};
}

window.payMeBack.login = (function() {
    var submitCredentials = function(email, password, isSignup) {
            alert("email="+email+", password="+password+" isSignup="+isSignup);
        var url, httpMethod, payload;
        if (isSignup === true) {
            httpMethod =  "GET";
            payload = "";
            url = window.payMeBack.core.makePathFromVirtual("~/api/authorisation?grant_type=password&username="+email+"&password="+password+"&scope=modify");
        } else {
            httpMethod = "POST";
            payload = {
                emailAddress: email,
                password: password
            };
            url = window.payMeBack.core.makePathFromVirtual("~/api/authorisation");
        }
        alert(url);
        $.ajax({
            url:url,
            method:httpMethod,
            success: function(result){
                debugger;
                alert("worked!");
            },
            error: function() {
                alert("failed");
            }
        });
    };

	var showLoginDialog = function (redirectToHomeOnClose, isSignUp) {
		var redirectOnClose = false;
        var isSignupAction = false;
        if (typeof redirectToHomeOnClose !== 'undefined') {
            redirectOnClose = redirectToHomeOnClose === true;
        }
        if (typeof isSignUp !== 'undefined') {
            isSignupAction = isSignUp === true;
        }

      $.nyroModalManual(
          {
              url: '#credentials-dialog',
              minHeight: 300,
              height: 300,
              //modal: true,
              //closeButton: null,
              endRemove: function() {
                  var credsElement = $("#credentials-container");
                  credsElement.hide();
                  $("li input",credsElement).val("");
                  if (redirectOnClose) {
                      location.reload(window.payMeBack.core.makePathFromVirtual("~"));
                  }

              },
              endShowContent: function () {
                  $("#credentials-submit").unbind().bind("click",function() {
                      var email = $("#credentials-userId").val();
                      var password = $("#credentials-userPassword").val();
                      submitCredentials(email,password,isSignupAction);
                  });
                  if (isSignupAction) {
                      $("#credentials-container")
                          .removeClass("login-dialog")
                          .addClass("signup-dialog")
                          .fadeIn();
                  } else {
                      $("#credentials-container")
                          .addClass("login-dialog")
                          .removeClass("signup-dialog")
                          .fadeIn();
                  }
              }
          });
  };

  return {showLoginDialog: function(redirectToHomeOnClose, isSignUp) {showLoginDialog(redirectToHomeOnClose,isSignUp)} };
})();
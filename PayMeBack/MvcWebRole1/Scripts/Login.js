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

window.payMeBack.login = (function () {

    var isUserSignedIn = function () {
        var state = $("#is-signed-in-state").val();
        return (typeof state !== 'undefined' && state === "true");
    };

    var updateDisplayBasedOnSignedInStatus = function (isSignedIn) {
        if (typeof isSignedIn === 'undefined') {
            isSignedIn = isUserSignedIn();
        }
        if (isSignedIn === true) {
            $(".hide-if-signed-in").hide();
            $(".show-if-signed-in").show();
        } else {
            $(".hide-if-signed-in").show();
            $(".show-if-signed-in").hide();
        }
    };
    var submitCredentials = function (email, password, isSignup) {
        var url, payload;
        payload = {
            email: email,
            password: password
        };
        if (isSignup === true) {
            url = window.payMeBack.core.makePathFromVirtual("~/membership/signup");
        } else {
            url = window.payMeBack.core.makePathFromVirtual("~/membership/login");
        }
        var jsonPayload;
        if (typeof JSON !== 'undefined' && typeof JSON.stringify !== 'undefined') {
            jsonPayload = JSON.stringify(payload);
        } else {
            jsonPayload = "";
        }

        $("#credentials-form").fadeOut('normal', function () {
            window.payMeBack.progressManager.showProgressIndicator("credentials-container");
            $.ajax({
                url: url,
                type: "POST",
                data: jsonPayload,
                contentType: 'application/json',
                dataType: "json",
                success: function (result) {
                    window.payMeBack.progressManager.hideProgressIndicator("credentials-container", function () {
                        if (result && typeof result.error !== 'undefined') {
                            window.payMeBack.notificationEngine.showStatusBarMessage("The request had an error: " + result.error, "#nyroModalContent", 5);
                            $("#credentials-form").fadeIn();

                        } else {
                            if (result && typeof result.success !== 'undefined' && result.success === true) {
                                // This worked
                                updateDisplayBasedOnSignedInStatus(true);
                                $.nyroModalRemove();
                            } else {
                                updateDisplayBasedOnSignedInStatus(false);
                                var msg = "";
                                if (typeof result.error !== 'undefined') {
                                    msg = result.error;
                                }

                                var msg;
                                if (isSignup === true) {
                                    msg = "Your request to signup was not successful. This user may already exist in the system or your credentials do not meet the minimum criteria";
                                } else {
                                    msg = "You could not be signed in as your credentials are not correct";
                                }
                                $("#credentials-form").fadeIn();

                                window.payMeBack.notificationEngine.showStatusBarMessage(msg, "#nyroModalContent");
                            }

                        }
                    });
                },
                error: function () {
                    window.payMeBack.progressManager.hideProgressIndicator("credentials-container", function () {
                        $("#credentials-form").fadeIn();
                        updateDisplayBasedOnSignedInStatus(false);

                        var msg = "There was a problem " + (isSignup ? "signing you up" : "logging you in") + " to the system. Please try again."
                        window.payMeBack.notificationEngine.showStatusBarMessage(msg, "#nyroModalContent", 5);
                    });
                }
            });
        });

    };

    var captureCredentialsAndSubmit = function (isSignupAction) {
        var email = $("#credentials-userId").val();
        var password = $("#credentials-userPassword").val();
        submitCredentials(email, password, isSignupAction);
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
              minHeight: 200,
              height: 200,
              minWidth: 350,
              width: 380,
              bgColor: "#A8A5A5",
              //modal: true,
              //closeButton: null,
              endRemove: function () {
                  var credsElement = $("#credentials-container");
                  credsElement.hide();
                  $("li input", credsElement).val("");
                  if (redirectOnClose) {

                      location.assign(window.payMeBack.core.makePathFromVirtual("~"));
                  }

              },
              endShowContent: function () {
                  $("#credentials-form input").unbind().on("keypress", function (e) {
                      if (e.which === 13) {
                          captureCredentialsAndSubmit(isSignupAction);
                      }
                  });
                  $("#credentials-submit").unbind().bind("click", function () {
                      captureCredentialsAndSubmit(isSignupAction);
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

                  $("#credentials-userId").focus();

              }
          });
    };

    return {
        showLoginDialog: function (redirectToHomeOnClose, isSignUp) { showLoginDialog(redirectToHomeOnClose, isSignUp) },
        updateDisplayBasedOnSignedInStatus: function (isSignedIn) { updateDisplayBasedOnSignedInStatus(isSignedIn); }
    };
})();

// This is the page load code which will determine whether login/sign in and sign out links are displayed or not
$(document).ready(function () {
    function bindLoginSignUpAction() {
        $("div.header-menu li a.credentials-link").unbind().bind("click", function (e) {
            var isSignUp = $(e.target).hasClass("sign-up");
            window.payMeBack.login.showLoginDialog(false, isSignUp);
        });
    }

    bindLoginSignUpAction();
    window.payMeBack.login.updateDisplayBasedOnSignedInStatus();

});
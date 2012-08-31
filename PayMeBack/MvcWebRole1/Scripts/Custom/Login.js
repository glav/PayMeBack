/// <reference path="../_references.js" />


if (typeof window.payMeBack.login === 'undefined') {
    window.payMeBack.login = {};
}

window.payMeBack.login = (function () {

    var isUserSignedIn = function () {
        var state = $("#is-signed-in-state").val();
        return (typeof state !== 'undefined' && state === "true");
    };

    var updateDisplayBasedOnSignedInStatus = function (isSignedIn, firstname) {
        if (typeof isSignedIn === 'undefined') {
            isSignedIn = isUserSignedIn();
        }
        if (isSignedIn === true) {
            $(".hide-if-signed-in").hide();
            $(".show-if-signed-in").show();
            if (typeof firstname !== 'undefined') {
                // If a firstname supplied attempt to bind it to any elements
                // that have a class of 'data-bind' and a {{firstname}} element in the text
                // NOTE: Knockout could probably be used here
                var textEl = $(".data-bind");
                if (textEl.length > 0) {
                    var replacedText = textEl.text().replace("{{firstname}}", firstname);
                    textEl.text(replacedText);
                }
            }
        } else {
            $(".hide-if-signed-in").show();
            $(".show-if-signed-in").hide();
        }
    };
    var submitCredentials = function (payload, isSignup) {
        var url;

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
                            window.payMeBack.notificationEngine.showStatusBarMessage("The request had an error: " + result.error, window.payMeBack.notificationEngine.MessageTypeError, "#nyroModalContent", 5);
                            $("#credentials-form").fadeIn();

                        } else {
                            if (result && typeof result.success !== 'undefined' && result.success === true) {
                                // This worked
                                updateDisplayBasedOnSignedInStatus(true, result.firstname);
                                $.nyroModalRemove();
                            } else {
                                updateDisplayBasedOnSignedInStatus(false);
                                var msg = "";
                                if (typeof result.error !== 'undefined') {
                                    msg = result.error;
                                }

                                var msg;
                                if (isSignup === true) {
                                    msg = "Your request to signup was not successful. This user may already exist in the system or your credentials do not meet the minimum criteria. " + msg;
                                } else {
                                    msg = "You could not be signed in as your credentials are not correct. " + msg;
                                }
                                $("#credentials-form").fadeIn();

                                window.payMeBack.notificationEngine.showStatusBarMessage(msg, window.payMeBack.notificationEngine.MessageTypeError, "#nyroModalContent");
                            }

                        }
                    });
                },
                error: function () {
                    window.payMeBack.progressManager.hideProgressIndicator("credentials-container", function () {
                        $("#credentials-form").fadeIn();
                        updateDisplayBasedOnSignedInStatus(false);

                        var msg = "There was a problem " + (isSignup ? "signing you up" : "logging you in") + " to the system. Please try again.";
                        window.payMeBack.notificationEngine.showStatusBarMessage(msg, window.payMeBack.notificationEngine.MessageTypeError, "#nyroModalContent", 5);
                    });
                }
            });
        });

    };

    var captureCredentialsAndSubmit = function (isSignupAction) {
        var payload = {};
        var email = $("#credentials-userId").val();
        var password = $("#credentials-userPassword").val();

        if (typeof email !== 'undefined' && typeof password !== 'undefined'
                && email.trim() !== "" && password.trim() !== "") {
            payload = {
                email: email,
                password: password
            };

            if (isSignupAction === true) {
                var firstName = $("#signup-firstname").val();
                var surname = $("#signup-surname").val();
                if (typeof firstName !== 'undefined') {
                    payload.firstname = firstName;
                } else {
                    payload.firstname = "";
                }
                if (typeof surname !== 'undefined') {
                    payload.surname = surname;
                } else {
                    payload.surname = "";
                }
            }
            submitCredentials(payload, isSignupAction);
        }
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

        var modalHeight = 200;
        if (isSignupAction) {
            modalHeight = 280;
        }

        $.nyroModalManual(
          {
              url: '#credentials-dialog',
              minHeight: modalHeight,
              height: modalHeight,
              minWidth: 350,
              width: 380,
              bgColor: window.payMeBack.core.colours.nyroModalBackground,
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
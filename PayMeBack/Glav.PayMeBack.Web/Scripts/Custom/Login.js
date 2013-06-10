/// <reference path="../_references.js" />


if (typeof window.payMeBack.login === 'undefined') {
    window.payMeBack.login = {};
}

window.payMeBack.login = (function () {

    var submitCredentials = function (payload, isSignup, onSuccessCallback) {
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
                                //updateDisplayBasedOnSignedInStatus(true, result.firstname);
                                $.nyroModalRemove();
                                $("#is-signed-in-state").val("true");
                                if (typeof onSuccessCallback !== 'undefined') {
                                    onSuccessCallback();
                                }
                            } else {
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
                        var msg = "There was a problem " + (isSignup ? "signing you up" : "logging you in") + " to the system. Please try again.";
                        window.payMeBack.notificationEngine.showStatusBarMessage(msg, window.payMeBack.notificationEngine.MessageTypeError, "#nyroModalContent", 5);
                    });
                }
            });
        });

    };


    var showLoginDialog = function (redirectToHomeOnClose, isSignUp, onSuccessCallback) {
        var redirectOnClose = false;
        var isSignupAction = false;
        if (typeof redirectToHomeOnClose !== 'undefined') {
            redirectOnClose = redirectToHomeOnClose === true;
        }
        if (typeof isSignUp !== 'undefined') {
            isSignupAction = isSignUp === true;
        }

        var modalHeight = 210;
        if (isSignupAction) {
            modalHeight = 340;
        }

        $.nyroModalManual(
          {
              url: '#credentials-dialog',
              minHeight: modalHeight,
              height: modalHeight,
              minWidth: 360,
              width: 380,
              bgColor: window.payMeBack.core.colours.nyroModalBackground,
              //modal: true,
              //closeButton: null,
              endRemove: function () {
                  var credsElement = $("#credentials-container");
                  credsElement.hide();
                  if (redirectOnClose) {
                      $("#intro-not-authenticated").fadeOut('slow', function () {
                          $("#intro-authenticated").fadeIn('slow', function () {
                              location.assign(window.payMeBack.core.makePathFromVirtual("~"));
                          });
                      });
                  }

              },
              endShowContent: function () {
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
        submitCredentials: submitCredentials,
        showLoginDialog: function (redirectToHomeOnClose, isSignUp, onSuccessCallback) { showLoginDialog(redirectToHomeOnClose, isSignUp, onSuccessCallback) }
    };
})();

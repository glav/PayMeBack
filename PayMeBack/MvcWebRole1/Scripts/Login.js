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

    var isUserSignedIn = function() {
        var state = $("#is-signed-in-state").val();
        return (typeof state !== 'undefined' && state === true);
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
		$.ajax({
			url: url,
			type: "POST",
			data: jsonPayload,
			contentType: 'application/json',
			dataType: "json",
			success: function (result) {
				if (result && typeof result.error !== 'undefined') {
					alert("The request had an error: " + result.error)
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
                            msg = "You could not be signed in asyour credentials are not correct";
                        }
						alert(msg);
					}

				}
			},
			error: function () {
				updateDisplayBasedOnSignedInStatus(false);

                var msg = "There was a problem " + (isSignup ? "signing you up" : "logging you in") + " to the system. Please try again."
				alert(msg);
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
          	endRemove: function () {
          		var credsElement = $("#credentials-container");
          		credsElement.hide();
          		$("li input", credsElement).val("");
          		if (redirectOnClose) {

          			location.assign(window.payMeBack.core.makePathFromVirtual("~"));
          		}

          	},
          	endShowContent: function () {
          		$("#credentials-submit").unbind().bind("click", function () {
          			var email = $("#credentials-userId").val();
          			var password = $("#credentials-userPassword").val();
          			submitCredentials(email, password, isSignupAction);
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

	return {
        showLoginDialog: function (redirectToHomeOnClose, isSignUp) { showLoginDialog(redirectToHomeOnClose, isSignUp) },
        updateDisplayBasedOnSignedInStatus: function(isSignedIn) { updateDisplayBasedOnSignedInStatus(isSignedIn);}
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
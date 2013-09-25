using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain.Services;
using System.Web.Security;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Domain.Engines;

namespace Glav.PayMeBack.Web.Domain
{
	public class WebMembershipManager : IWebMembershipManager
	{
		private IOAuthSecurityService _oAuthSecurityService;
		private ISignupManager _signupManager;
		private IUserEngine _userEngine;

		public WebMembershipManager(IOAuthSecurityService oAuthSecurityService, ISignupManager signupManager, IUserEngine userEngine)
		{
			_oAuthSecurityService = oAuthSecurityService;
			_signupManager = signupManager;
			_userEngine = userEngine;
		}

        public MembershipResponseModel SignupAndIssueCookie(string email, string password, string firstname, string surname)
		{
            var serviceResponse = new MembershipResponseModel { Firstname = firstname, Surname = surname, Email = email };
            var response = _signupManager.SignUpNewUser(email, firstname, surname, password);
            serviceResponse.IsSuccessfull = response.IsSuccessfull;
            if (response.IsSuccessfull)
            {
                CreateCookieWithAuthTokenAndSetResponse(email, response.AccessGrant.access_token, response.AccessGrant.refresh_token);
            }
            else
            {
                serviceResponse.IsSuccessfull = false;
                serviceResponse.Error = response.ErrorDetails.error;
            }

            return serviceResponse;
		}

		public MembershipResponseModel LoginAndIssueCookie(string email, string password)
		{
            var serviceResponse = new MembershipResponseModel { Email = email };

			var response = _oAuthSecurityService.AuthorisePasswordCredentialsGrant(email, password, "modify");
            serviceResponse.IsSuccessfull = response.IsSuccessfull;
            if (response.IsSuccessfull)
			{
                serviceResponse.AccessToken = response.AccessGrant.access_token;
                serviceResponse.RefreshToken = response.AccessGrant.refresh_token;
				CreateCookieWithAuthTokenAndSetResponse(email, response.AccessGrant.access_token, response.AccessGrant.refresh_token);
                var user = _userEngine.GetUserByEmail(email);
                serviceResponse.Firstname = user.FirstNames;
                serviceResponse.Surname = user.Surname;
            }
            return serviceResponse;
		}

		private void CreateCookieWithAuthTokenAndSetResponse(string email, string accessToken, string refreshToken)
		{
			var authCookie = FormsAuthentication.GetAuthCookie(email, false);
			var userData = string.Format("AccessToken;{0};RefreshToken;{1}", accessToken, refreshToken);
			var ticket = new FormsAuthenticationTicket(1, FormsAuthentication.FormsCookieName, DateTime.Now, DateTime.Now.AddSeconds(FormsAuthentication.Timeout.TotalSeconds), false, userData);
			authCookie.Value = FormsAuthentication.Encrypt(ticket);
			HttpContext.Current.Response.Cookies.Add(authCookie);
            SetAccessTokenInResponseHeader(accessToken);
		}

        private void SetAccessTokenInResponseHeader(string accessToken)
        {
            if (HttpContext.Current.Response.Headers.AllKeys.Contains("Bearer"))
            {
                HttpContext.Current.Response.Headers["Bearer"] = accessToken;
            }
            else
            {
                HttpContext.Current.Response.Headers.Add("Bearer", accessToken);
            }
        }

		public User GetUserFromRequestCookie()
		{
			User user = null;
			var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (cookie != null)
			{
				var authTicket = FormsAuthentication.Decrypt(cookie.Value);
				var userDataList = authTicket.UserData.Split(';');
				if (userDataList.Length >= 4)
				{
					var accessToken = userDataList[1];
					var refreshToken = userDataList[3];
					if (!_oAuthSecurityService.IsAccessTokenValid(accessToken))
					{
						accessToken = ReIssueAuthCookieWithRefreshedToken(cookie,authTicket, refreshToken);
					}
                    SetAccessTokenInResponseHeader(accessToken);
                    return _userEngine.GetUserByAccessToken(accessToken);
				}
			}
			return user;
		}

		private string ReIssueAuthCookieWithRefreshedToken(HttpCookie existingCookie,FormsAuthenticationTicket authTicket, string refreshToken)
		{
			var refreshedAccess = _oAuthSecurityService.RefreshAccessToken(refreshToken,"modify");
			var userData = string.Format("AccessToken;{0};RefreshToken;{1}", refreshedAccess.AccessGrant.access_token, refreshedAccess.AccessGrant.refresh_token);
			var ticket = new FormsAuthenticationTicket(authTicket.Version, authTicket.Name, authTicket.IssueDate, DateTime.Now.AddSeconds(FormsAuthentication.Timeout.TotalSeconds), false, userData);
			existingCookie.Value = FormsAuthentication.Encrypt(ticket);
			if (HttpContext.Current.Response.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
			{
				HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
			}
			HttpContext.Current.Response.Cookies.Add(existingCookie);

			return refreshedAccess.AccessGrant.access_token;
		}

		public void Logout()
		{
			FormsAuthentication.SignOut();
		}
	}
}
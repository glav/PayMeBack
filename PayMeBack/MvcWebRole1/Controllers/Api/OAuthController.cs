using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Web.Controllers.Api
{
    public class OAuthController : ApiController
    {
		private IOAuthSecurityService _oAuthSecurityService;
		private ISignupManager _signupManager;

		public OAuthController(IOAuthSecurityService oAuthSecurityService, ISignupManager signupManager)
		{
			_oAuthSecurityService = oAuthSecurityService;
			_signupManager = signupManager;
		}
        //
        // GET: /OAuth/

		public object GetAuthorisationCodeGrant(string response_type, string client_id, string scope, string state)
		{
			// TODO: move this to external service
			if (response_type != "code")
			{
				throw new ArgumentException(OAuthErrorResponseCode.InvalidRequest);
				// Need to ensure we return a valid OAuth2 complient error response with invalid code error
			}
			return new HttpResponseMessage() { Content = new StringContent(string.Format("response_type=[{0}], client_id=[{1}], scope=[{2}], state=[{3}]", response_type, client_id, scope, state))};

			//return new HttpResponseMessage(string.Format("response_type=[{0}], client_id=[{1}], scope=[{2}], state=[{3}]", response_type, client_id, scope, state));
		}

		/// <summary>
		/// The primary way of signing into the system and getting an access token issued
		/// to enable further access to the system. This is typically a one time process
		/// with the refresh token acting for subsequent access to the system.
		/// </summary>
		/// <param name="grant_type"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="scope"></param>
		/// <returns></returns>
		public object GetAuthorisationPasswordCredentialsGrant(string grant_type, string username, string password, string scope)
		{
			// TODO: move this to external service
			if (grant_type != "password")
			{
				throw new ArgumentException(OAuthErrorResponseCode.InvalidGrant);
			}

			// validate grant type - ensure it is password
			var response = _oAuthSecurityService.AuthorisePasswordCredentialsGrant(username, password, scope);
			if (response.IsSuccessfull)
			{
				return response.AccessGrant;
			}

			return response.ErrorDetails;
		}

		public object GetRefreshedAccessToken(string grant_type, string refresh_token, string scope)
		{
			// TODO: move this to external service
			if (grant_type != "refresh_token")
			{
				throw new ArgumentException(OAuthErrorResponseCode.InvalidGrant);
				// Need to ensure we return a valid OAuth2 complient error response with invalid code error
			}

			// validate grant type - ensure it is password
			var response = _oAuthSecurityService.RefreshAccessToken(refresh_token, scope);
			if (response.IsSuccessfull)
			{
				return response.AccessGrant;
			}

			return response.ErrorDetails;
		}

		/// <summary>
		/// This is provided simply to be able to test whether the access/bearer token issued is valid
		/// </summary>
		/// <returns></returns>
		public bool HeadPing()
		{
			return true;
		}

		public object PostSignUpDetails(SignupData signupData)
		{
			var response = new OAuthAuthorisationGrantResponse();
			try
			{
				response = _signupManager.SignUpNewUser(signupData.emailAddress, signupData.firstNames, signupData.lastName, signupData.password);
				if (response.IsSuccessfull)
				{
					return response.AccessGrant;
				}

				return response.ErrorDetails;
			}
			catch (Exception ex)
			{
				response.IsSuccessfull = false;
				response.ErrorDetails = new OAuthGrantRequestError { error = OAuthErrorResponseCode.InvalidClient };
				return response.ErrorDetails;
			}
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
		private ISignupManager _signupService;

		public OAuthController(IOAuthSecurityService oAuthSecurityService, ISignupManager signupService)
		{
			_oAuthSecurityService = oAuthSecurityService;
			_signupService = signupService;
		}
        //
        // GET: /OAuth/

		public object GetAuthorisationCodeGrant(string response_type, string client_id, string scope, string state)
		{
			// TODO: move this to external service
			if (response_type != "code")
			{
				throw new ArgumentException("invalid response type");
				// Need to ensure we return a valid OAuth2 complient error response with invalid code error
			}
			return new HttpResponseMessage() { Content = new StringContent(string.Format("response_type=[{0}], client_id=[{1}], scope=[{2}], state=[{3}]", response_type, client_id, scope, state))};

			//return new HttpResponseMessage(string.Format("response_type=[{0}], client_id=[{1}], scope=[{2}], state=[{3}]", response_type, client_id, scope, state));
		}

		public object GetAuthorisationPasswordCredentialsGrant(string grant_type, string username, string password, string scope)
		{
			// TODO: move this to external service
			if (grant_type != "password")
			{
				throw new ArgumentException("invalid grant type");
				// Need to ensure we return a valid OAuth2 complient error response with invalid code error
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
				throw new ArgumentException("invalid grant type");
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
		[System.Web.Http.HttpGet]
		public bool Ping()
		{
			return true;
		}

		public object PostSignUpDetails(SignupData signupData)
		{
			var response = new OAuthAuthorisationGrantResponse();
			try
			{
				response = _signupService.SignUpNewUser(signupData.emailAddress, signupData.firstNames, signupData.lastName, signupData.password);
				if (response.IsSuccessfull)
				{
					return response.AccessGrant;
				}

				return response.ErrorDetails;
			}
			catch (Exception ex)
			{
				response.IsSuccessfull = false;
				response.ErrorDetails = new OAuthGrantRequestError { error = "invalid_client" };
				return response.ErrorDetails;
			}
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using Glav.PayMeBack.Core;
using System.Net.Http;

namespace Glav.PayMeBack.Client.Proxies
{
	public class AuthorisationProxy : BaseProxy
	{
		public AuthorisationProxy() : base()
		{
			PagingEnabled = false;
		}

		public AuthorisationProxy(string bearerToken) : base(bearerToken)
		{
			PagingEnabled = false;
		}

		public override string RequestPrefix
		{
			get { return ResourceNames.Authorisation; }
		}

		/// <summary>
		/// Resource owner password credentials grant
		/// (Section 4.3 of OAuth2 Draft spec)
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public ProxyResponse<OAuthAuthorisationGrantResponse> PasswordCredentialsGrantRequest(string username, string password, string scope = "")
		{
			var result = new OAuthAuthorisationGrantResponse();

			ContentType = RequestContentType.ApplicationJson;
			var uri = base.GetRequestUri(string.Format("?grant_type=password&username={0}&password={1}&scope={2}",username,password,scope));
			var response = GetResponse(uri);
			if (response.IsRequestSuccessfull)
			{
				result = ParseRawOAuthResponse(response.RawResponse);
			}
			else
			{
				result.IsSuccessfull = false;
			}

			var statusCode = result.IsSuccessfull ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;
			return new ProxyResponse<OAuthAuthorisationGrantResponse>(response.RawResponse, result, result.IsSuccessfull, statusCode, string.Empty);
		}

		/// <summary>
		/// Resource owner Authorisation code grant
		/// (Section 4.1 of OAuth2 Draft spec)
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public ProxyResponse<OAuthAuthorisationGrantResponse> AuthorisationCodeGrantRequest(string client_id, string scope = "", string state = "")
		{
			var result = new OAuthAuthorisationGrantResponse();

			ContentType = RequestContentType.ApplicationJson;
			var uri = base.GetRequestUri(string.Format("?response_type=code&client_id={0}&scope={1}&state={2}", client_id, scope, state));
			var response = GetResponse(uri);

			if (response.IsRequestSuccessfull)
			{
				result = ParseRawOAuthResponse(response.RawResponse);
			}
			else
			{
				result.IsSuccessfull = false;
			}

			var statusCode = result.IsSuccessfull ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;
			return new ProxyResponse<OAuthAuthorisationGrantResponse>(response.RawResponse, result, result.IsSuccessfull, statusCode, string.Empty);

		}

		public ProxyResponse<OAuthAuthorisationGrantResponse> RefreshAccessToken(string refreshToken, string scope)
		{
			var result = new OAuthAuthorisationGrantResponse();

			ContentType = RequestContentType.ApplicationJson;
			var uri = base.GetRequestUri(string.Format("?grant_type=refresh_token&refresh_token={0}&scope={1}", refreshToken, scope));
			var response = GetResponse(uri);

			if (response.IsRequestSuccessfull)
			{
				result = ParseRawOAuthResponse(response.RawResponse);
			}
			else
			{
				result.IsSuccessfull = false;
			}

			var statusCode = result.IsSuccessfull ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;
			return new ProxyResponse<OAuthAuthorisationGrantResponse>(response.RawResponse, result, result.IsSuccessfull, statusCode, string.Empty);

			
		}

		private OAuthAuthorisationGrantResponse ParseRawOAuthResponse(string rawResponse)
		{
			var result = new OAuthAuthorisationGrantResponse();
			try
			{
				JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
				var successDto = jsonSerializer.Deserialize<OAuthAccessTokenGrant>(rawResponse);
				if (successDto != null && !string.IsNullOrWhiteSpace(successDto.access_token))
				{
					result.IsSuccessfull = true;
					result.AccessGrant = successDto;
					return result;
				}
				var failureDto = jsonSerializer.Deserialize<OAuthGrantRequestError>(rawResponse);
				// If we try and deserialise and get bugger all, then form up a default error response
				if (failureDto == null || string.IsNullOrWhiteSpace(failureDto.error))
				{
					failureDto = new OAuthGrantRequestError { error = "invalid_client" };
				}
				result.IsSuccessfull = false;
				result.ErrorDetails = failureDto;
			}
			catch
			{
				JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
				var dto = jsonSerializer.Deserialize<OAuthGrantRequestError>(rawResponse);
				result.IsSuccessfull = false;
				result.ErrorDetails = dto;
			}

			return result;
		}

		public ProxyResponse<string> AuthorisationPing()
		{
			var originalOperationType = OperationMethod;
			OperationMethod = HttpMethod.Head;
			ContentType = RequestContentType.ApplicationJson;
			
			var uri = base.GetRequestUri("Ping");
			var response = base.GetResponse<string>(uri);

			OperationMethod = originalOperationType;
			return response;
		}

		public ProxyResponse<OAuthAuthorisationGrantResponse> Signup(string emailAddress, string firstNames, string lastName, string password)
		{
			var result = new OAuthAuthorisationGrantResponse();

			ContentType = RequestContentType.ApplicationJson;
			
			var uri = base.GetRequestUri("");

			var originalOperation = OperationMethod;
			OperationMethod = HttpMethod.Post;

			var postData = new SignupData { emailAddress = emailAddress, firstNames = firstNames, lastName = lastName, password = password };
			var response = base.GetResponse<SignupData,object>(uri, postData);
			OperationMethod = originalOperation;
			if (response.IsRequestSuccessfull)
			{
				result = ParseRawOAuthResponse(response.RawResponse);
			}
			else
			{
				result.IsSuccessfull = false;
			}

			return new ProxyResponse<OAuthAuthorisationGrantResponse>(response.RawResponse, result, response.IsRequestSuccessfull,response.StatusCode,response.ReasonCode);
		}
	}

}

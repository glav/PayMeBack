using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using Glav.PayMeBack.Core;

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
				//TODO: This serialisation thing below doesn't work properly. The deserialisation of the OAuthAccessTokenGrant
				// works even if an error structure is passed through. Need to parse it better
				try
				{
					JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
					var dto = jsonSerializer.Deserialize<OAuthAccessTokenGrant>(response.RawResponse);
					result.IsSuccessfull = true;
					result.AccessGrant = dto;
				}
				catch
				{
					JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
					var dto = jsonSerializer.Deserialize<OAuthGrantRequestError>(response.RawResponse);
					result.IsSuccessfull = false;
					result.ErrorDetails = dto;
				}
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

			//TODO: The deserialisation and casting is incorrect for the authorisation code grant
			// and needs to be changed
			if (response.IsRequestSuccessfull)
			{
				try
				{
					JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
					var dto = jsonSerializer.Deserialize<OAuthAccessTokenGrant>(response.RawResponse);
					result.IsSuccessfull = true;
					result.AccessGrant = dto;
				}
				catch
				{
					JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
					var dto = jsonSerializer.Deserialize<OAuthGrantRequestError>(response.RawResponse);
					result.IsSuccessfull = false;
					result.ErrorDetails = dto;
				}
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

			//TODO: The deserialisation and casting is incorrect for the authorisation code grant
			// and needs to be changed
			if (response.IsRequestSuccessfull)
			{
				try
				{
					JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
					var dto = jsonSerializer.Deserialize<OAuthAccessTokenGrant>(response.RawResponse);
					result.IsSuccessfull = true;
					result.AccessGrant = dto;
				}
				catch
				{
					JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
					var dto = jsonSerializer.Deserialize<OAuthGrantRequestError>(response.RawResponse);
					result.IsSuccessfull = false;
					result.ErrorDetails = dto;
				}
			}
			else
			{
				result.IsSuccessfull = false;
			}

			var statusCode = result.IsSuccessfull ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;
			return new ProxyResponse<OAuthAuthorisationGrantResponse>(response.RawResponse, result, result.IsSuccessfull, statusCode, string.Empty);

			
		}

		public ProxyResponse<string> AuthorisationPing()
		{
			ContentType = RequestContentType.ApplicationJson;
			var uri = base.GetRequestUri("Ping");
			return base.GetResponse<string>(uri);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Web.Domain.Engines
{
	public static class OAuthExtensions
	{
		public static AuthorisationRequestType ToAuthRequestType(this string responseType)
		{
			switch (responseType.ToLowerInvariant())
			{
				case "code":
					return AuthorisationRequestType.Code;
					break;
				case "password":
					return AuthorisationRequestType.PasswordCredentials;
					break;
				default:
					return AuthorisationRequestType.None;
					break;

			}
		}

		public static bool IsOAuthExplicitGrantRequest(this HttpRequestMessage request)
		{
			if (request == null)
			{
				return false;
			}
			var lowerUri = request.RequestUri.PathAndQuery.ToLowerInvariant();
			if (lowerUri.StartsWith(string.Format("/{0}",ResourceNames.Authorisation)) 
					&& !lowerUri.StartsWith(string.Format("/{0}",ResourceNames.SecurePing)))
			{
				return true;
			}

			return false;
		}
		
		public static bool DoesContainOAuthToken(this HttpRequestMessage request)
		{
			if (request == null)
			{
				return false;
			}

			// Check for authorisation/access/bearer token that will need validation
			if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Bearer")
			{
				return true;
			}

			return false;
		}
	}

}

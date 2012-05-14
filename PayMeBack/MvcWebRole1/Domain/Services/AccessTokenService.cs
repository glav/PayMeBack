using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class AccessTokenService : IAccessTokenService
	{
		public Guid ExtractTokenFromQueryString(string url)
		{
			var uri = new Uri(url);
			var pairs = uri.ParseQueryString();
			if (pairs != null && pairs.HasKeys())
			{
				var accessTokenValue = pairs[QueryStringConstants.AccessToken];
				var accessToken = new Guid(accessTokenValue);
				return accessToken;
			}

			return Guid.Empty; ;
		}

		public Guid TryExtractToken()
		{
			if (System.Web.HttpContext.Current != null)
			{
				return ExtractTokenFromQueryString(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
			}
			return Guid.Empty;
		}
	}
}
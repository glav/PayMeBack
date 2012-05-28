using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.PayMeBack.Core
{
	public static class OAuthErrorResponseCode
	{
		public const string InvalidRequest = "invalid_request";
		public const string InvalidClient = "invalid_client";
		public const string InvalidGrant = "invalid_grant";
		public const string UnauthorizedClient = "unauthorized_client";
		public const string UnsupportedGrantType = "unsupported_grant_type";
		public const string InvalidScope = "invalid_scope";
	}
}

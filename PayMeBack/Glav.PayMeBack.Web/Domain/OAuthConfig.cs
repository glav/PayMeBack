using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public static class OAuthConfig
	{
		public const int AccessTokenExpiryInMinutes = 60;
		public const int RefreshTokenExpiryInDays = 0;  // never expires
	}
}
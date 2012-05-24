using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.PayMeBack.Core
{
	public class OAuthAuthorisationGrantResponse
	{
		public bool IsSuccessfull { get; set; }
		public OAuthGrantRequestError ErrorDetails { get; set; }
		public OAuthAccessTokenGrant AccessGrant { get; set; }
	}

	public class OAuthAccessTokenGrant
	{
		public string access_token { get; set; }
		public string token_type { get; set; }
		public int expires_in { get; set; }
		public string refresh_token { get; set; }
		public string scope { get; set; }
	}

	public class OAuthGrantRequestError
	{
		public string error { get; set; }
		public string error_description { get; set; }
	}
}

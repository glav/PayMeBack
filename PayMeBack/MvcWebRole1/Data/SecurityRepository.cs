using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	public class SecurityRepository : ISecurityRepository
	{
		public OAuthToken Insert(OAuthToken token)
		{
			throw new NotImplementedException();
		}

		public void Update(OAuthToken token)
		{
			throw new NotImplementedException();
		}

		public OAuthToken GetTokenDataByRefreshToken(string token)
		{
			throw new NotImplementedException();
		}


		public OAuthToken GetTokenDataByAccessToken(string token)
		{
			throw new NotImplementedException();
		}
	}
}
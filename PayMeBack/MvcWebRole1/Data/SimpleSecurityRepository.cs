using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	public class SimpleSecurityRepository : ISimpleSecurityRepository
	{
		public AccessToken CreateAccessTokenForUser(Guid userId)
		{
			// create a new token and store in DB for this user
			var accessToken = new AccessToken { Token = Guid.NewGuid(), UserId = Guid.NewGuid(), TokenExpiry = DateTime.Now.AddMinutes(1) };
			return accessToken;
		}

		public AccessToken GetAccessToken(Guid tokenId)
		{
			return new AccessToken();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Data;
using System.Security;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class SecurityService : ISecurityService
	{
		private ISimpleSecurityRepository _securityRepository;

		public SecurityService(ISimpleSecurityRepository securityRepository)
		{
			_securityRepository = securityRepository;
		}
		
		public string CreateHashValue(string input)
		{
			//TODO:return a proper hash value
			return input;
		}


		public AccessToken CreateAccessToken(Guid userId, Guid tokenId)
		{
			var token = _securityRepository.CreateAccessTokenForUser(userId);
			if (token == null || token.Token == Guid.Empty || token.TokenExpiry < DateTime.UtcNow)
			{
				throw new SecurityException("Invalid access token");
			}

			return token;
		}


		public bool IsAccessTokenValid(Guid accessTokenId)
		{
			var token = _securityRepository.GetAccessToken(accessTokenId);

			return (token != null
						&& token.UserId != Guid.Empty
						&& token.Token != Guid.Empty
						&& DateTime.UtcNow <= token.TokenExpiry);
		}
	}
}
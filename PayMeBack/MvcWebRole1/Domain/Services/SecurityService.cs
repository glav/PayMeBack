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
		private ISecurityRepository _securityRepository;

		public SecurityService(ISecurityRepository securityRepository)
		{
			_securityRepository = securityRepository;
		}
		
		public string CreateHashValue(string input)
		{
			//TODO:return a proper hash value
			return input;
		}


		public Guid ValidateAndRenewToken(Guid userId, Guid tokenId)
		{
			var renewedToken = _securityRepository.RenewTokenForUserIfOldTokenValid(userId, tokenId);
			if (renewedToken == null || renewedToken == Guid.Empty)
			{
				throw new SecurityException("Invalid access token");
			}

			return renewedToken;
		}
	}
}
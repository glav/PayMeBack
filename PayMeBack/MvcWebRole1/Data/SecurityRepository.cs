using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Data
{
	public class SecurityRepository : ISecurityRepository
	{
		public Guid CreateAccessTokenForUser(Guid userId)
		{
			return Guid.NewGuid();
		}

		public Guid RenewTokenForUserIfOldTokenValid(Guid userId, Guid oldToken)
		{
			return Guid.NewGuid();
		}
	}
}
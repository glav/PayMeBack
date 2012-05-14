using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Data
{
	public interface ISecurityRepository
	{
		Guid CreateAccessTokenForUser(Guid userId);
		Guid RenewTokenForUserIfOldTokenValid(Guid userId, Guid oldToken);
	}
}
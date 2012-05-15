using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	/// <summary>
	/// This interface is for our custom security token management. Ideally OAuth
	/// should be used but we should provide the functionality to use just a username/pwd
	/// defined in this app too.
	/// </summary>
	public interface ISimpleSecurityRepository
	{
		AccessToken CreateAccessTokenForUser(Guid userId);
		AccessToken GetAccessToken(Guid tokenId);
	}
}
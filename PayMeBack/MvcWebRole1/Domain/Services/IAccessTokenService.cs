using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface IAccessTokenService
	{
		Guid ExtractTokenFromQueryString(string url);
		Guid TryExtractToken();
	}
}
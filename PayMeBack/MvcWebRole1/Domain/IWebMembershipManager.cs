using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Web.Domain
{
	public interface IWebMembershipManager
	{
		bool SignupAndIssueCookie(string email, string password);
		bool LoginAndIssueCookie(string email, string password);
		User GetUserFromRequestCookie();
		void Logout();
	}
}
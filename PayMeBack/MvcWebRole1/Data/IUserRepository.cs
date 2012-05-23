using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	public interface IUserRepository
	{
		UserDetail GetUser(string emailAddress);
		UserDetail GetUser(Guid userId);
		Guid AddUser(User user , string password);
		string GetUserPassword(string emailAddress);
	}
}
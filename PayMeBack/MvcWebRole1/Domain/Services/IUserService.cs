using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface IUserService
	{
		User GetUser(string emailAddress);
		void SaveOrUpdateUser(User user);
		void DeleteUser(User user);
		void RegisterUser(User user, string password);
	}
}
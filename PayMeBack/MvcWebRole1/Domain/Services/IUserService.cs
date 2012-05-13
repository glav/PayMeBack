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
		Guid RegisterUser(string emailAddress, string firstNames, string lastName, string password);
	}
}
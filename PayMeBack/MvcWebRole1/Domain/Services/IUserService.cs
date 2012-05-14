using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface IUserService
	{
		User GetUserByEmail(string emailAddress);
		User GetUserById(Guid id);
		void SaveOrUpdateUser(User user);
		void DeleteUser(User user);
		Guid RegisterUser(string emailAddress, string firstNames, string lastName, string password);
	}
}
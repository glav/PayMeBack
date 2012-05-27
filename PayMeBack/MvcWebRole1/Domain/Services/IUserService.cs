using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface IUserService
	{
		User GetUserByEmail(string emailAddress);
		User GetUserById(Guid id);
		void SaveOrUpdateUser(User user, string password = null);
		void DeleteUser(User user);
		OAuthAuthorisationGrantResponse RegisterUser(string emailAddress, string firstNames, string lastName, string password);
	}
}
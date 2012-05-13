using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	public class MainRepository : IRepository
	{
		public Domain.User GetUser(string emailAddress)
		{
			//TODO: Stub for now
			return new User {EmailAddress = emailAddress, Id = Guid.NewGuid()};
		}

		public User GetUser(Guid userId)
		{
			//TODO: Stub for now
			return new User { Id = userId };
		}

		public string GetUserPassword(string emailAddress)
		{
			//TODO: Stub for now
			return string.Empty;
		}


		public Guid AddUser(User user, string password)
		{
			//TODO: Stub for now
			return Guid.NewGuid();
		}
	}
}
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
			return new User {EmailAddress = emailAddress};
		}

		public User GetUser(Guid userId)
		{
			//TODO: Stub for now
			return new User { Id = userId };
		}


		public Guid AddUser(User user)
		{
			//TODO: Stub for now
			return Guid.NewGuid();
		}
	}
}
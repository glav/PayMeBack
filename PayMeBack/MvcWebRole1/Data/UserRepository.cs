using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	public class UserRepository : IUserRepository
	{
		public UserDetail GetUser(string emailAddress)
		{
			using (var context = new PayMeBackEntities())
			{
				return context.UserDetails.FirstOrDefault(u => u.EmailAddress == emailAddress);
			}
		}

		public UserDetail GetUser(Guid userId)
		{
			using (var context = new PayMeBackEntities())
			{
				return context.UserDetails.FirstOrDefault(u => u.Id == userId);
			}
		}

		public string GetUserPassword(string emailAddress)
		{
			var user = GetUser(emailAddress);
			if (user != null)
			{
				return user.Password;
			}

			return string.Empty;
		}


		public Guid AddUser(User user, string password)
		{
			using (var context = new PayMeBackEntities())
			{
				var detail = new UserDetail()
				{
					EmailAddress = user.EmailAddress,
					FirstNames = user.FirstNames,
					Password = password,
					Surname = user.Surname
				};

				context.UserDetails.Add(detail);
				context.SaveChanges();
				return detail.Id;
			}
		}


	}
}
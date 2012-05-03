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
			return new User {EmailAddress = emailAddress};
		}
	}
}
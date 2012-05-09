using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	public interface IRepository
	{
		User GetUser(string emailAddress);
		User GetUser(Guid userId);
		Guid AddUser(User user);
	}
}
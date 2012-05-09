using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface ISignInService
	{
		User SignIn(string email, string password);
	}
}
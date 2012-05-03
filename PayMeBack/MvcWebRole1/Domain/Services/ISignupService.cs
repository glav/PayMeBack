using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface ISignupService
	{
		Guid SignUpNewUser(string emailAddress, string firstNames, string lastName, string password);
	}
}
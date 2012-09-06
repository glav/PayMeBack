using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface ISignupManager
	{
		OAuthAuthorisationGrantResponse SignUpNewUser(string emailAddress, string firstNames, string lastName, string password);
		void SetUserToValidated(Guid userId);
	}
}
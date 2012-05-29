using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Engines
{
	public interface IEmailEngine
	{
		bool IsValidEmail(string emailAddress);
	}
}
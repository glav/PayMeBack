using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Engines
{
	public class EmailEngine : IEmailEngine
	{
		public bool IsValidEmail(string emailAddress)
		{
			return true;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class EmailService : IEmailService
	{
		public bool IsValidEmail(string emailAddress)
		{
			return true;
		}
	}
}
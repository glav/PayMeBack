using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.PayMeBack.Web.Domain.Services;

namespace PayMeBackWeb.UnitTests.Controllers.ApiControllerTests
{
	public class MockEmailService : IEmailService
	{
		public bool IsValidEmail(string emailAddress)
		{
			return true;
		}
	}
}

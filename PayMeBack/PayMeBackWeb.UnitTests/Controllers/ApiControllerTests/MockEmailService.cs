using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain.Engines;

namespace PayMeBackWeb.UnitTests.Controllers.ApiControllerTests
{
	public class MockEmailService : IEmailEngine
	{
		public bool IsValidEmail(string emailAddress)
		{
			return true;
		}
	}
}

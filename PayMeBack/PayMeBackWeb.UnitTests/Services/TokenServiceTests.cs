using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain;

namespace PayMeBackWeb.UnitTests.Services
{
	[TestClass]
	public class TokenServiceTests
	{
		[TestMethod]
		public void ShouldExtractTokenFromFromQueryString()
		{
			var tokenService = new SimpleAccessTokenService();
			var originalToken = Guid.NewGuid();

			var queryString = string.Format("http://localhost/?{0}={1}",QueryStringConstants.AccessToken,originalToken.ToString());
			var token = tokenService.ExtractTokenFromQueryString(queryString);
			Assert.AreEqual<Guid>(originalToken, token);
		}
	}
}

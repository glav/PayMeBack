using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Domain.Services;

namespace PayMeBackWeb.UnitTests.Services
{
	[TestClass]
	public class TokenServiceTests
	{
		[TestMethod]
		public void ShouldExtractTokenFromFromQueryString()
		{
			var tokenService = new AccessTokenService();
			var originalToken = Guid.NewGuid();
			var token = tokenService.ExtractTokenFromQueryString("http://localhost/?accesstoken=" + originalToken.ToString());
			Assert.AreEqual<Guid>(originalToken, token);
		}
	}
}

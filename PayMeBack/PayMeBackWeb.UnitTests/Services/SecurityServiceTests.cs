using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Data;
using Moq;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain;

namespace PayMeBackWeb.UnitTests.Services
{
	[TestClass]
	public class SecurityServiceTests
	{
		[TestMethod]
		public void AccessTokenShouldBeValid()
		{
			var validTokenId = Guid.NewGuid();

			var repo = new Mock<ISimpleSecurityRepository>();
			repo.Setup<AccessToken>(m => m.GetAccessToken(validTokenId)).Returns(new AccessToken{ Token= validTokenId, UserId = Guid.NewGuid(), TokenExpiry = DateTime.UtcNow.AddMinutes(1)});

			var service =new SecurityService(repo.Object);

			Assert.IsTrue(service.IsAccessTokenValid(validTokenId), "Access Token should be valid");
		}

		[TestMethod]
		public void AccessTokenShouldNotBeValidAsDoesNotExist()
		{
			var tokenId = Guid.NewGuid();

			var repo = new Mock<ISimpleSecurityRepository>();
			repo.Setup<AccessToken>(m => m.GetAccessToken(tokenId)).Returns<AccessToken>(null);

			var service = new SecurityService(repo.Object);

			Assert.IsFalse(service.IsAccessTokenValid(Guid.NewGuid()), "Access Token should be valid as it does not exist");
		}

		[TestMethod]
		public void AccessTokenShouldNotBeValidAsExpired()
		{
			var validTokenId = Guid.NewGuid();

			var repo = new Mock<ISimpleSecurityRepository>();
			repo.Setup<AccessToken>(m => m.GetAccessToken(validTokenId)).Returns(new AccessToken { Token = validTokenId, TokenExpiry = DateTime.UtcNow.AddMinutes(-1), UserId = Guid.NewGuid() });

			var service = new SecurityService(repo.Object);

			Assert.IsFalse(service.IsAccessTokenValid(validTokenId), "Access Token should NOT be valid as it has expired");
		}

	}
}

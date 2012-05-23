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
		private Mock<IUserRepository> _userRepo;
		private Mock<ISecurityRepository> _securityRepo;
		private OAuthSecurityService _securityService;

		[TestMethod]
		[Ignore]
		public void AccessTokenShouldBeValid()
		{
			var validTokenId = Guid.NewGuid();
			InitialiseOauthSecurityService();

			_securityRepo.Setup<OAuthToken>(m => m.GetTokenDataByAccessToken(validTokenId.ToString())).Returns(new OAuthToken { AccessToken = validTokenId.ToString(), AssociatedUserId = Guid.NewGuid(), AccessTokenExpiry = DateTime.UtcNow.AddMinutes(1) });

			Assert.IsTrue(_securityService.IsAccessTokenValid(validTokenId.ToString()), "Access Token should be valid");
		}

		[TestMethod]
		public void AccessTokenShouldNotBeValidAsDoesNotExist()
		{
			var tokenId = Guid.NewGuid();
			InitialiseOauthSecurityService();

			_securityRepo.Setup<OAuthToken>(m => m.GetTokenDataByAccessToken(tokenId.ToString())).Returns<OAuthToken>(null);

			Assert.IsFalse(_securityService.IsAccessTokenValid(Guid.NewGuid().ToString()), "Access Token should be valid as it does not exist");
		}

		[TestMethod]
		public void AccessTokenShouldNotBeValidAsExpired()
		{
			var validTokenId = Guid.NewGuid();
			InitialiseOauthSecurityService();
			_securityRepo.Setup<OAuthToken>(m => m.GetTokenDataByAccessToken(validTokenId.ToString())).Returns(new OAuthToken { AccessToken = validTokenId.ToString(), AccessTokenExpiry = DateTime.UtcNow.AddMinutes(-1), AssociatedUserId = Guid.NewGuid() });

			Assert.IsFalse(_securityService.IsAccessTokenValid(validTokenId.ToString()), "Access Token should NOT be valid as it has expired");
		}

		private void InitialiseOauthSecurityService()
		{
			_securityRepo = new Mock<ISecurityRepository>();
			_userRepo = new Mock<IUserRepository>();
			_securityService = new OAuthSecurityService(_securityRepo.Object, _userRepo.Object);
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Data;
using Moq;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain;
using Glav.CacheAdapter.Core;
using System.Linq.Expressions;

namespace PayMeBackWeb.UnitTests.Services
{
	[TestClass]
	public class SecurityServiceTests
	{
		private Mock<ICrudRepository> _crudRepo;
		private OAuthSecurityService _securityService;
		private Mock<ICacheProvider> _cacheProvider;

		[TestMethod]
		public void AccessTokenShouldBeValid()
		{
			var validTokenId = Guid.NewGuid();
			InitialiseOauthSecurityService();

			var token = new OAuthToken { AccessToken = validTokenId.ToString(), AssociatedUserId = Guid.NewGuid(), AccessTokenExpiry = DateTime.UtcNow.AddMinutes(1) };
			_crudRepo.Setup<OAuthToken>(m => m.GetSingle<OAuthToken>(It.IsAny<Expression<Func<OAuthToken,bool>>>())).Returns(token);
			_cacheProvider.Setup<OAuthToken>(m => m.Get<OAuthToken>(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<Func<OAuthToken>>())).Returns(token);
			Assert.IsTrue(_securityService.IsAccessTokenValid(validTokenId.ToString()), "Access Token should be valid");
		}

		[TestMethod]
		public void AccessTokenShouldNotBeValidAsDoesNotExist()
		{
			var tokenId = Guid.NewGuid();
			InitialiseOauthSecurityService();

			_crudRepo.Setup<OAuthToken>(m => m.GetSingle<OAuthToken>(t => t.AccessToken == tokenId.ToString())).Returns<OAuthToken>(null);

			Assert.IsFalse(_securityService.IsAccessTokenValid(Guid.NewGuid().ToString()), "Access Token should be valid as it does not exist");
		}

		[TestMethod]
		public void AccessTokenShouldNotBeValidAsExpired()
		{
			var validTokenId = Guid.NewGuid();
			InitialiseOauthSecurityService();
	
			_crudRepo.Setup<OAuthToken>(m => m.GetSingle<OAuthToken>(t => t.AccessToken == validTokenId.ToString())).Returns(new OAuthToken { AccessToken = validTokenId.ToString(), AccessTokenExpiry = DateTime.UtcNow.AddMinutes(-1), AssociatedUserId = Guid.NewGuid() });

			Assert.IsFalse(_securityService.IsAccessTokenValid(validTokenId.ToString()), "Access Token should NOT be valid as it has expired");
		}

		private void InitialiseOauthSecurityService()
		{
			_crudRepo = new Mock<ICrudRepository>();
			_cacheProvider = new Mock<ICacheProvider>();
			_securityService = new OAuthSecurityService(_crudRepo.Object,_cacheProvider.Object);
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Controllers.Api;
using Glav.PayMeBack.Web.Data;
using Moq;
using Glav.CacheAdapter.Core;
using System.Linq.Expressions;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Web.Domain.Engines;

namespace PayMeBackWeb.UnitTests.Controllers.ApiControllerTests
{
	[TestClass]
	public class OAuthControllerTests
	{
		private ISignupManager _signupService;
		private Mock<ICrudRepository> _crudRepo;
		private IOAuthSecurityService _securityService;
		private Mock<ICacheProvider> _cacheProvider;
		private OAuthController _controller;
		private Mock<IUserRepository> _userRepo;

		[TestMethod]
		[Ignore]  // This needs to be moved to integration tests
		public void ShouldSignIn()
		{
			// Cannot mock a single get as it will attempt to retrieve later
			// Need to NOT mock the DB and allow. This should be integration test
			_crudRepo.Setup(m => m.GetSingle<UserDetail>(It.IsAny<Expression<Func<UserDetail,bool>>>())).Returns<UserDetail>(null);

			var signupDetails = new SignupData { emailAddress = string.Format("{0}@unittests.com",Guid.NewGuid()), firstNames="new", lastName="user", password="password"};
			var signUpResponse = _controller.PostSignUpDetails(signupDetails);
			var realResponse = signUpResponse as OAuthAccessTokenGrant;
			Assert.IsNotNull(signUpResponse,"No response from OAuth controller");
			Assert.IsNotNull(realResponse, "No access token grant response from OAuth controller");
			Assert.IsFalse(string.IsNullOrWhiteSpace(realResponse.access_token),"No valid access token returned");
		}

		[TestInitialize]
		public void InitControllers()
		{
			_crudRepo = new Mock<ICrudRepository>();

			_cacheProvider = new Mock<ICacheProvider>();
			_securityService = new OAuthSecurityService(_crudRepo.Object,_cacheProvider.Object);
			_userRepo = new Mock<IUserRepository>();
			_signupService = new SignupManager(new UserEngine(_crudRepo.Object, _securityService,_userRepo.Object), _securityService);
			_controller = new OAuthController(_securityService, _signupService);
		}

	}

}

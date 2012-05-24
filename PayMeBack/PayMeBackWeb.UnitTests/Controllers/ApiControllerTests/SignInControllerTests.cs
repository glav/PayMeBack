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

namespace PayMeBackWeb.UnitTests.Controllers.ApiControllerTests
{
	[TestClass]
	public class SignInControllerTests
	{
		private ISignupService _signupService;
		private SignUpController _signUpController;
		private SimpleSignInController _signInController;
		private Mock<ICrudRepository> _crudRepo;
		private IOAuthSecurityService _securityService;
		private Mock<ICacheProvider> _cacheProvider;

		[TestMethod]
		public void ShouldSignIn()
		{
			_crudRepo.Setup(m => m.GetSingle<UserDetail>(It.IsAny<Expression<Func<UserDetail,bool>>>())).Returns(new UserDetail { EmailAddress = "exists@domain.com", FirstNames = "exists", Surname = "domain", Id = Guid.NewGuid(), Password="password" });

			var signInResponse = _signInController.PostSignInDetails("exists@domain.com", "password");
			Assert.IsNotNull(signInResponse,"No response from Sign in controller");
			Assert.IsNotNull(signInResponse.AccessToken);
			Assert.AreNotEqual<Guid>(Guid.Empty,signInResponse.AccessToken);
		}

		[TestInitialize]
		public void InitControllers()
		{
			_crudRepo = new Mock<ICrudRepository>();

			_cacheProvider = new Mock<ICacheProvider>();
			_securityService = new OAuthSecurityService(_crudRepo.Object,_cacheProvider.Object);
			_signupService = new SignupService(new MockEmailService(), new UserService(_crudRepo.Object, _securityService));
			_signUpController = new SignUpController(_signupService);
			_signInController = new SimpleSignInController(_securityService);
		}

	}

}

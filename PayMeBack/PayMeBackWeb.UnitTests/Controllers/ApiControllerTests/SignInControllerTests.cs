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

namespace PayMeBackWeb.UnitTests.Controllers.ApiControllerTests
{
	[TestClass]
	public class SignInControllerTests
	{
		private Mock<IUserRepository> _userRepository;
		private ISignupService _signupService;
		private SignUpController _signUpController;
		private SimpleSignInController _signInController;
		private Mock<ISecurityRepository> _securityRepoCustom;
		private IOAuthSecurityService _securityService;

		[TestMethod]
		public void ShouldSignIn()
		{
			var signInResponse = _signInController.PostSignInDetails("exists@domain.com", "password");
			Assert.IsNotNull(signInResponse,"No response from Sign in controller");
			Assert.IsNotNull(signInResponse.AccessToken);
			Assert.AreNotEqual<Guid>(Guid.Empty,signInResponse.AccessToken);
		}

		[TestInitialize]
		public void InitControllers()
		{
			_userRepository = new Mock<IUserRepository>();
			_securityRepoCustom = new Mock<ISecurityRepository>();

			_userRepository.Setup<UserDetail>(m => m.GetUser("exists@domain.com")).Returns(new UserDetail { EmailAddress = "exists@domain.com", FirstNames = "exists", Surname = "domain", Id = Guid.NewGuid() });
			_userRepository.Setup<string>(m => m.GetUserPassword("exists@domain.com")).Returns("password");

			_securityService = new OAuthSecurityService(_securityRepoCustom.Object, _userRepository.Object);

			_signupService = new SignupService(new MockEmailService(), new UserService(_userRepository.Object,_securityService));
			_signUpController = new SignUpController(_signupService);
			_signInController = new SimpleSignInController(_securityService);
		}

	}

}

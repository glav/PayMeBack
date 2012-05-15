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
		private ISignInService _signInService;
		private Mock<ISimpleSecurityRepository> _securityRepoCustom;

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
			_securityRepoCustom = new Mock<ISimpleSecurityRepository>();

			_securityRepoCustom.Setup<AccessToken>(m => m.CreateAccessTokenForUser(It.IsAny<Guid>())).Returns(new AccessToken { Token = Guid.NewGuid(), TokenExpiry = DateTime.UtcNow.AddMinutes(10), UserId = Guid.NewGuid() });
			_userRepository.Setup<User>(m => m.GetUser("exists@domain.com")).Returns(new User { EmailAddress = "exists@domain.com", FirstNames = "exists", Surname = "domain", Id = Guid.NewGuid() });
			_userRepository.Setup<string>(m => m.GetUserPassword("exists@domain.com")).Returns("password");

			_signupService = new SignupService(new MockEmailService(), new UserService(_userRepository.Object, new SecurityService(_securityRepoCustom.Object)));
			_signUpController = new SignUpController(_signupService);
			_signInService = new SignInService(_userRepository.Object,new SecurityService(_securityRepoCustom.Object));
			_signInController = new SimpleSignInController(_signInService);
		}

	}

}

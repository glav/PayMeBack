using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Controllers.Api;
using Glav.PayMeBack.Web.Data;

namespace PayMeBackWeb.UnitTests.Controllers.ApiControllerTests
{
	[TestClass]
	public class SignInControllerTests
	{
		private IUserRepository _repository;
		private ISignupService _signupService;
		private SignUpController _signUpController;
		private SignInController _signInController;
		private ISignInService _signInService;

		[TestMethod]
		public void ShouldSignIn()
		{
			var signInResponse = _signInController.PostSignInDetails("exists@domain.com", "password");
			Assert.IsNotNull(signInResponse,"No response from Sign in controller");
			Assert.IsNotNull(signInResponse.UserId);
			Assert.AreNotEqual<Guid>(Guid.Empty,signInResponse.UserId);
		}

		[TestInitialize]
		public void InitControllers()
		{
			_repository = new MockRepository();
			_signupService = new SignupService(new MockEmailService(), new UserService(_repository, new SecurityService()));
			_signUpController = new SignUpController(_signupService);
			_signInService = new SignInService(_repository,new SecurityService());
			_signInController = new SignInController(_signInService);
		}

	}

}

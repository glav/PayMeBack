using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Domain.Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Helpers;
using Autofac;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.IntegrationTests.ServiceTests
{
	[TestClass]
	public class SignupServiceTests
	{
		private ISignupManager _signupMgr;
		private IOAuthSecurityService _securityService;
		private IUserEngine _userEngine;

		[TestMethod]
		public void SHouldBeAbleToSignUpAndSignIn()
		{
			SignUpThenSignIn();
		}
		
		[TestMethod]
		public void ShouldNotBeAbleToSignInWhenUserNotValidated()
		{
			BuildServices();

			var email = string.Format("test{0}@test.com", Guid.NewGuid());
			var invalidUser = new User
			                  	{
									FirstNames = "some",
									Surname = "dude",
									EmailAddress=email
			                  	};
			_userEngine.SaveOrUpdateUser(invalidUser,"password");
			var response = _securityService.AuthorisePasswordCredentialsGrant(email, "password", string.Empty);
			Assert.IsNotNull(response);
			Assert.IsFalse(response.IsSuccessfull);
			Assert.IsNotNull(response.ErrorDetails);
			Assert.IsFalse(string.IsNullOrWhiteSpace(response.ErrorDetails.error));
		}

		[TestMethod]
		public void ShouldNotBeAbleToSignInWithEmptyPasswordEvenIfValidated()
		{
			BuildServices();

			var emailAddress = string.Format("{0}@integrationtests.com", Guid.NewGuid());
			var signUpResponse = _signupMgr.SignUpNewUser(emailAddress, "integration", "test", "");
			var signInResponse = _securityService.SignIn(emailAddress, "");
			Assert.IsNull(signInResponse);
		}

		public User SignUpThenSignIn()
		{
			BuildServices();

			var emailAddress = string.Format("{0}@integrationtests.com", Guid.NewGuid());
			var signUpResponse = _signupMgr.SignUpNewUser(emailAddress, "integration", "test", "password");

			Assert.IsNotNull(signUpResponse);
			Assert.IsTrue(signUpResponse.IsSuccessfull);
			Assert.IsNotNull(signUpResponse.AccessGrant);
			Assert.IsFalse(string.IsNullOrWhiteSpace(signUpResponse.AccessGrant.access_token));

			var signInResponse = _securityService.SignIn(emailAddress, "password");
			return signInResponse;
		}



		private void BuildServices()
		{
			var builder = new WebDependencyBuilder();
			var container = builder.BuildDependencies();

			_signupMgr = container.Resolve<ISignupManager>();
			_securityService = container.Resolve<IOAuthSecurityService>();
			_userEngine = container.Resolve<IUserEngine>();
		}
	}
}

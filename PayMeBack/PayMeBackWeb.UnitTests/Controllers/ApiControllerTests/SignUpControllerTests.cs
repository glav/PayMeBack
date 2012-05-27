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
using Glav.PayMeBack.Core;

namespace PayMeBackWeb.UnitTests.Controllers.ApiControllerTests
{
	[TestClass]
	public class SignUpControllerTests
	{
		[TestMethod]
		public void ShouldSignUp()
		{
			var controller = GetController();
			var response = controller.PostSignUpDetails("newuser@somewhere.com", "new", "user", "passwod");
			
			Assert.IsNotNull(response,"No response from Signup Controller");
			var realResponse = response as OAuthGrantRequestError;
			Assert.IsNotNull(realResponse);
			Assert.IsFalse(string.IsNullOrWhiteSpace(realResponse.error));
		}

		private static SignUpController GetController()
		{
			var crudRepository = new Mock<ICrudRepository>();
			var cacheProvider = new Mock<ICacheProvider>();

			crudRepository.Setup<UserDetail>(m => m.GetSingle<UserDetail>(u => u.EmailAddress == "exists@domain.com")).Returns(new UserDetail { EmailAddress = "exists@domain.com", FirstNames = "exists", Surname = "domain", Id = Guid.NewGuid() });

			var securityService = new OAuthSecurityService(crudRepository.Object, cacheProvider.Object);

			var service = new SignupService(new MockEmailService(), new UserService(crudRepository.Object, securityService));
			var controller = new SignUpController(service);
			return controller;
		}

		[TestMethod]
		public void ShouldNotSignUpIfAlreadySignedUp()
		{
			//TODO: Should ideally pre-register a user in the repo
			var controller = GetController();
			var response = controller.PostSignUpDetails("exists@domain.com", "exists", "domain", "password");
			Assert.IsNotNull(response, "No response from sign up controller");
			var realResponse = response as OAuthGrantRequestError;
			Assert.IsNotNull(realResponse);
			Assert.IsFalse(string.IsNullOrWhiteSpace(realResponse.error),"Registered a user that already exists");
		}
	}

	public class MockEmailService : IEmailService
	{
		public bool IsValidEmail(string emailAddress)
		{
			return true;
		}
	}



}

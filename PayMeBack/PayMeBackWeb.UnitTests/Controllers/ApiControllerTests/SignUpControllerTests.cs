﻿using System;
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
	public class SignUpControllerTests
	{
		[TestMethod]
		public void ShouldSignUp()
		{
			var controller = GetController();
			var response = controller.PostSignUpDetails("newuser@somewhere.com", "new", "user", "passwod");
			
			Assert.IsNotNull(response,"No response from Signup Controller");
			Assert.IsNotNull(response.UserId);
		}

		private static SignUpController GetController()
		{
			var mockRepo = new MockRepository();
			var service = new SignupService(new MockEmailService(), new UserService(mockRepo, new SecurityService()));
			var controller = new SignUpController(service);
			return controller;
		}

		[TestMethod]
		public void ShouldNotSignUpIfAlreadySignedUp()
		{
			//TODO: Should ideally pre-register a user in the repo
			var controller = GetController();
			var response = controller.PostSignUpDetails("exists@domain.com", "exists", "domain", "password");
			Assert.IsNotNull(response,"No response from sign up controller");
			Assert.IsFalse(response.IsSuccessful,"Registered a user that already exists");
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
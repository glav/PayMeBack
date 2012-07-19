using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Domain.Engines;

namespace Glav.PayMeBack.IntegrationTests.EngineTests
{
	[TestClass]
	public class UserEngineTests
	{
		private IUserEngine _userEngine;

		[TestMethod]
		public void CannotSaveUserWithoutEmail()
		{
			BuildServices();

			try
			{
				_userEngine.SaveOrUpdateUser(new User
												{
													Id = Guid.Empty,
													FirstNames = "test",
													Surname = "user"
												},"password");
				Assert.Fail("Should not be able to save user without email address");
			}
			catch
			{
			}
		}

		[TestMethod]
		public void ShouldBeAbleToSaveUnvalidatedUserWithOnlyEmailAddress()
		{
			BuildServices();

			var email = string.Format("test-{0}@tests.com", Guid.NewGuid());
			try
			{
				_userEngine.SaveOrUpdateUser(new User
												{
													EmailAddress = email
												});
			}
			catch (Exception ex)
			{
				Assert.Fail(string.Format("Should be able to save user with just a unique email address. {0}",ex.Message));
			}
		}

		[TestMethod]
		public void ShouldNotBeAbleToSaveNewUnvalidatedUserWithSameEmailAsExistingUser()
		{
			BuildServices();

			var email = string.Format("test-{0}@tests.com", Guid.NewGuid());
			try
			{
				// this should work
				_userEngine.SaveOrUpdateUser(new User
				{
					EmailAddress = email
				});
				// this should fail
				_userEngine.SaveOrUpdateUser(new User
				{
					EmailAddress = email
				});
				Assert.Fail("Should NOT be able to save user with a non unique email address.");

			}
			catch (Exception ex)
			{
			}
			
		}

		private void BuildServices()
		{
			var builder = new WebDependencyBuilder();
			var container = builder.BuildDependencies();

			_userEngine = container.Resolve<IUserEngine>();
		}
	}
}

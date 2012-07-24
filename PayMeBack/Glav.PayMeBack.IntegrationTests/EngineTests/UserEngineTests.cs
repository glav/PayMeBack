using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.IntegrationTests.EngineTests
{
	[TestClass]
	public class UserEngineTests
	{
		private IUserEngine _userEngine;
		private ICrudRepository _crudRepo;

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

		[TestMethod]
		public void ShouldReturnAllUsers()
		{
			BuildServices();

			ClearAllData();

			var users = _crudRepo.GetAll<UserDetail>();
			Assert.AreEqual<int>(0,users.Count());

			AddTestUsers(false);

			var result= _userEngine.SearchUsers(null, null);
			Assert.AreEqual<int>(3,result.Count());
		}

		[TestMethod]
		public void ShouldReturnOnlyUsersMatchingSearchByCriteria()
		{
			BuildServices();

			ClearAllData();

			var users = _crudRepo.GetAll<UserDetail>();
			Assert.AreEqual<int>(0, users.Count());

			AddTestUsers(true);

			var searchResult = _userEngine.SearchUsers(null, "tuna");
			Assert.AreEqual<int>(1,searchResult.Count());

			searchResult = _userEngine.SearchUsers(null, "one@one");
			Assert.AreEqual<int>(1, searchResult.Count());

			searchResult = _userEngine.SearchUsers(null, "iNtEgRaTion");
			Assert.AreEqual<int>(1, searchResult.Count());

		}

		[TestMethod]
		public void ShouldReturnOnlyUsersMatchingSearchByCriteriaUsingPaging()
		{
			BuildServices();

			ClearAllData();

			var users = _crudRepo.GetAll<UserDetail>();
			Assert.AreEqual<int>(0, users.Count());

			AddTestUsers(true);

			var searchResult = _userEngine.SearchUsers(null, null);
			Assert.AreEqual<int>(new RequestPagingFilter().PageSize, searchResult.Count());

			var pageTestFilter = new RequestPagingFilter {Page = 2, PageSize = 10};
			searchResult = _userEngine.SearchUsers(pageTestFilter,null );
			Assert.AreEqual<int>(pageTestFilter.PageSize, searchResult.Count());

			searchResult = _userEngine.SearchUsers(pageTestFilter, "lastname");
			Assert.AreEqual<int>(pageTestFilter.PageSize, searchResult.Count());

		}

		private void AddTestUsers(bool includeUsersForPaging)
		{
			_userEngine.SaveOrUpdateUser(new User
			                             	{
			                             		FirstNames = "test",
			                             		Surname = "User",
			                             		EmailAddress = "one@One.com"
			                             	});
			_userEngine.SaveOrUpdateUser(new User
			                             	{
			                             		FirstNames = "Integration",
			                             		Surname = "Test",
			                             		EmailAddress = "two@test.com"
			                             	});
			_userEngine.SaveOrUpdateUser(new User
			                             	{
			                             		FirstNames = "Bumblebee",
			                             		Surname = "Tuna",
			                             		EmailAddress = "ace@ventura.com"
			                             	});
			if (includeUsersForPaging)
			{
				for (var cnt = 0; cnt < 30; cnt++)
				{
					_userEngine.SaveOrUpdateUser(new User
					                             	{
					                             		FirstNames = string.Format("User{0}", cnt),
					                             		Surname = string.Format("Lastname{0}", cnt),
					                             		EmailAddress = string.Format("User{0}@test{0}.com", cnt)
					                             	});
				}
			}
		}

		private void ClearAllData()
		{
// Delete all existing Data
			_crudRepo.Delete<DebtPaymentInstallmentDetail>(d => true);
			_crudRepo.Delete<DebtDetail>(dd => true);
			_crudRepo.Delete<UserPaymentPlanDetail>(up => true);
			_crudRepo.Delete<UserDetail>(u => true);
		}

		private void BuildServices()
		{
			var builder = new WebDependencyBuilder();
			var container = builder.BuildDependencies();

			_userEngine = container.Resolve<IUserEngine>();
			_crudRepo = container.Resolve<ICrudRepository>();
		}
	}
}

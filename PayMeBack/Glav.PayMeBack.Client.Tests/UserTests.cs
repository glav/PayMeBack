using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.PayMeBack.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Client.Proxies;

namespace Glav.PayMeBack.Client.Tests
{
	[TestClass]
	public class UserTests
	{
		[TestMethod]
		public void ShouldBeAbleToSearchForUsers()
		{
			var emailList = new List<string>();
			var prefix = Guid.NewGuid().ToString();
			// Signup 10 users so we can search them
			for (var cnt = 0; cnt < 10; cnt++)
			{
				var email = string.Format("{0}_{1}@integrationtest_{1}.com", prefix,cnt);
				SignUpUser(email, string.Format("{0}-test-srch",prefix), "dude");
				emailList.Add(email);				
			}

			// Now sign up me -the searcher :-)
			var token = SignUpUser(Guid.NewGuid().ToString(), "The", "Searcher");
			var userProxy = new UserProxy(token);

			// Search for 1 user
			var searchResponse = userProxy.Search(emailList[0], null);
			Assert.IsNotNull(searchResponse);
			Assert.IsTrue(searchResponse.IsRequestSuccessfull);
			Assert.IsNotNull(searchResponse.DataObject);
			Assert.AreEqual<int>(1, searchResponse.DataObject.Count());

			// Search for a all users (except me)
			searchResponse = userProxy.Search(prefix, null);
			Assert.IsNotNull(searchResponse);
			Assert.IsTrue(searchResponse.IsRequestSuccessfull);
			Assert.IsNotNull(searchResponse.DataObject);
			Assert.AreEqual<int>(10, searchResponse.DataObject.Count());

			// Search for a few users with paging
			searchResponse = userProxy.Search("test-srch-", new RequestPagingFilter { Page = 1, PageSize = 5 });
			Assert.IsNotNull(searchResponse);
			Assert.IsTrue(searchResponse.IsRequestSuccessfull);
			Assert.IsNotNull(searchResponse.DataObject);
			Assert.AreEqual<int>(5, searchResponse.DataObject.Count());

			// Search for a few users with bad criteria
			searchResponse = userProxy.Search("zzxxyy", null);
			Assert.IsNotNull(searchResponse);
			Assert.IsTrue(searchResponse.IsRequestSuccessfull);
			Assert.IsNotNull(searchResponse.DataObject);
			Assert.AreEqual<int>(0, searchResponse.DataObject.Count());
		}

		private string SignUpUser(string email, string firstName, string lastName)
		{
			var authProxy = new AuthorisationProxy();
			var response = authProxy.Signup(email, firstName, lastName, "P@ssw0rd1");
			Assert.IsNotNull(response);
			Assert.IsTrue(response.IsRequestSuccessfull);
			Assert.IsNotNull(response.DataObject);
			Assert.IsNotNull(response.DataObject.AccessGrant);
			Assert.IsFalse(string.IsNullOrWhiteSpace(response.DataObject.AccessGrant.access_token));
			return response.DataObject.AccessGrant.access_token;
		}
	}
}

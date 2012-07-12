using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Client.Proxies;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Client.Tests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class DebtTests
	{
		[TestMethod]
		public void ShouldBeAbleToRetrieveNewPlan()
		{
			var proxy = new AuthorisationProxy();
			var response = proxy.Signup(Guid.NewGuid().ToString(),"test","dude", "P@ssw0rd1");

			Assert.IsNotNull(response);
			Assert.IsTrue(response.IsRequestSuccessfull);
			Assert.IsNotNull(response.DataObject);
			Assert.IsNotNull(response.DataObject.AccessGrant);
			Assert.IsFalse(string.IsNullOrWhiteSpace(response.DataObject.AccessGrant.access_token));

			proxy.BearerToken = response.DataObject.AccessGrant.access_token;

			Assert.Inconclusive("Still need to make call to GetPlan");
		}


		[TestMethod]
		public void ShouldBeAbleToUpdatePlan()
		{
			var proxy = new AuthorisationProxy();
			var email = string.Format("{0}@integrationtest.com",Guid.NewGuid());
			var response = proxy.Signup(email, "test","dude","P@ssw0rd1");

			Assert.IsNotNull(response);
			Assert.IsTrue(response.IsRequestSuccessfull);
			Assert.IsNotNull(response.DataObject);
			Assert.IsNotNull(response.DataObject.AccessGrant);
			Assert.IsFalse(string.IsNullOrWhiteSpace(response.DataObject.AccessGrant.access_token));
		
			Assert.Inconclusive("Need to get plan, make amendments, then update plan");
		}

	}
}

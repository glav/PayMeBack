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
			var email = string.Format("{0}@integrationtest.com",Guid.NewGuid());
			var response = proxy.Signup(email,"test","dude", "P@ssw0rd1");

			Assert.IsNotNull(response);
			Assert.IsTrue(response.IsRequestSuccessfull);
			Assert.IsNotNull(response.DataObject);
			Assert.IsNotNull(response.DataObject.AccessGrant);
			Assert.IsFalse(string.IsNullOrWhiteSpace(response.DataObject.AccessGrant.access_token));

			proxy.BearerToken = response.DataObject.AccessGrant.access_token;

			var debtProxy = new DebtProxy(response.DataObject.AccessGrant.access_token);

			//TODO: Where de we get the userId from?
			//TODO:Should we just allow the server to parse it from the passed in access token
			var planResponse = debtProxy.GetDebtPaymentPlan();
			Assert.IsNotNull(planResponse);
			Assert.IsTrue(planResponse.IsRequestSuccessfull);
			Assert.IsNotNull(planResponse.DataObject);
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

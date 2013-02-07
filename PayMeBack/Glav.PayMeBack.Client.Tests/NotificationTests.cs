using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Client.Proxies;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Client.Tests
{
	[TestClass]
	public class NotificationTests
	{
		[TestMethod]
		public void ShouldBeAbleToRetrieveNotificationOptions()
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

			var notifyProxy = new NotificationsProxy(response.DataObject.AccessGrant.access_token);

            var notifyResponse = notifyProxy.GetNotificationOptions();
            Assert.IsNotNull(notifyResponse);
            Assert.IsTrue(notifyResponse.IsRequestSuccessfull);
            Assert.IsNotNull(notifyResponse.DataObject);
		}

		[TestMethod]
		public void ShouldBeAbleToUpdateNotificationOptions()
		{
			var proxy = new AuthorisationProxy();
			var ownerEmail = string.Format("{0}@integrationtest.com",Guid.NewGuid());
			var owesDebtEmail = string.Format("{0}@integrationtest.com",Guid.NewGuid());

			// Sign up owner of primary account - person whois owed a debt
			var response = proxy.Signup(ownerEmail, "test","dude","P@ssw0rd1");

			Assert.IsNotNull(response);
			Assert.IsTrue(response.IsRequestSuccessfull);
			Assert.IsNotNull(response.DataObject);
			Assert.IsNotNull(response.DataObject.AccessGrant);
			Assert.IsFalse(string.IsNullOrWhiteSpace(response.DataObject.AccessGrant.access_token));
			var token = response.DataObject.AccessGrant.access_token;

			// Now sign up person who owes dent
			var secondSignUpResponse = proxy.Signup(owesDebtEmail, "I", "OweDebt", "P@ssw0rd1");
			Assert.IsNotNull(secondSignUpResponse);
			Assert.IsTrue(secondSignUpResponse.IsRequestSuccessfull);

			var debtProxy = new DebtProxy(response.DataObject.AccessGrant.access_token);
			debtProxy.BearerToken = token;

			var planResponse = debtProxy.GetDebtPaymentPlan();
			Assert.IsNotNull(planResponse);
			Assert.IsTrue(planResponse.IsRequestSuccessfull);

			var uniqueEmail = string.Format("test-{0}@debttests.com", Guid.NewGuid());
			var debtToAdd = new Debt
								{
									DateCreated = DateTime.Now,
									InitialPayment = 20,
									TotalAmountOwed = 100,
									UserWhoOwesDebt = new User { EmailAddress=uniqueEmail }
								};
			planResponse.DataObject.DebtsOwedToMe.Add(debtToAdd);
			var updateResponse = debtProxy.UpdatePaymentPlan(planResponse.DataObject);
			Assert.IsNotNull(updateResponse);
			Assert.IsTrue(updateResponse.IsRequestSuccessfull);

			var updatedPlanResponse = debtProxy.GetDebtPaymentPlan();
			Assert.IsNotNull(updatedPlanResponse);
			Assert.IsTrue(updatedPlanResponse.IsRequestSuccessfull);
			Assert.IsNotNull(updatedPlanResponse.DataObject.DebtsOwedToMe);
			Assert.AreEqual<int>(1, updatedPlanResponse.DataObject.DebtsOwedToMe.Count);

		}

        private string SetupValidUserAndAuthenticate()
        {
            var proxy = new AuthorisationProxy();
            var email = string.Format("{0}@integrationtest.com", Guid.NewGuid());
            var response = proxy.Signup(email, "test", "dude", "P@ssw0rd1");

            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsRequestSuccessfull);
            Assert.IsNotNull(response.DataObject);
            Assert.IsNotNull(response.DataObject.AccessGrant);
            Assert.IsFalse(string.IsNullOrWhiteSpace(response.DataObject.AccessGrant.access_token));

            proxy.BearerToken = response.DataObject.AccessGrant.access_token;

            return response.DataObject.AccessGrant.access_token;
        }

	}
}

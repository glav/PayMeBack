﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Client.Proxies;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;

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

			var planResponse = debtProxy.GetDebtPaymentPlan();
			Assert.IsNotNull(planResponse);
			Assert.IsTrue(planResponse.IsRequestSuccessfull);
			Assert.IsNotNull(planResponse.DataObject);
		}

		[TestMethod]
		public void ShouldNotBeAbleToRetrieveNewPlanWithInvalidToken()
		{
			var debtProxy = new DebtProxy("1234567890");

			var planResponse = debtProxy.GetDebtPaymentPlan();
			Assert.IsNotNull(planResponse);
			Assert.IsFalse(planResponse.IsRequestSuccessfull);
		}

		[TestMethod]
		public void ShouldBeAbleToUpdatePlan()
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

			var debtToAdd = new Debt
								{
									DateCreated = DateTime.Now,
									InitialPayment = 20,
									TotalAmountOwed = 100,
									UserWhoOwesDebt = new User { EmailAddress="new@user.com" }
								};
			planResponse.DataObject.DebtsOwedToMe.Add(debtToAdd);

			Assert.Inconclusive("Need to get plan, make amendments, then update plan");
		}

	}
}
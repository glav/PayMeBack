﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Helpers;
using Autofac;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.IntegrationTests.ServiceTests
{
	[TestClass]
	public class PaymentPlanServiceTests
	{
		private ISignupManager _signupMgr;
		private IPaymentPlanService _paymentPlanService;
		private IOAuthSecurityService _securityService;

		[TestMethod]
		public void ShouldBeAbleToSignInAndGetUserPaymentPlan()
		{
			BuildServices();

			SignUpSignInAndGetNewPlan();
		}


		[TestMethod]
		public void ShouldBeAbleToAddDebtToUserPaymentPlan()
		{
			BuildServices();

			var user = SignUpSignInAndGetNewPlan();
			var paymentPlan = _paymentPlanService.GetPaymentPlan(user.Id);

			var emailAddress = string.Format("owes-debt-{0}@integrationtests.com", Guid.NewGuid());
			_signupMgr.SignUpNewUser(emailAddress, "I", "owe", "password");
			var userWhoOwesDebt = _securityService.SignIn(emailAddress, "password");

			paymentPlan.DebtsOwedToMe.Add(new Debt
				{
					ReasonForDebt="test",
					TotalAmountOwed=100,
					InitialPayment=10,
					PaymentPeriod= PaymentPeriod.Weekly,
					StartDate = DateTime.Now,
					UserWhoOwesDebt = userWhoOwesDebt,
				});
			var result = _paymentPlanService.UpdatePaymentPlan(paymentPlan);
			Assert.IsNotNull(result);
			Assert.IsTrue(result.WasSuccessfull);

			var planToCheck = _paymentPlanService.GetPaymentPlan(user.Id);
			Assert.IsNotNull(planToCheck);
			Assert.IsNotNull(planToCheck.DebtsOwedToMe);
			Assert.IsNotNull(planToCheck.DebtsOwedToOthers);
			Assert.IsTrue(paymentPlan.DateCreated.Day == planToCheck.DateCreated.Day);
			Assert.IsTrue(paymentPlan.DateCreated.Month == planToCheck.DateCreated.Month);
			Assert.IsTrue(paymentPlan.DateCreated.Year == planToCheck.DateCreated.Year);
			Assert.IsTrue(paymentPlan.DateCreated.Hour == planToCheck.DateCreated.Hour);
			Assert.IsTrue(paymentPlan.DateCreated.Minute == planToCheck.DateCreated.Minute);
			Assert.IsTrue(paymentPlan.DateCreated.Second == planToCheck.DateCreated.Second);
			Assert.AreEqual<int>(paymentPlan.DebtsOwedToMe.Count,planToCheck.DebtsOwedToMe.Count);
			Assert.AreEqual<int>(paymentPlan.DebtsOwedToOthers.Count, planToCheck.DebtsOwedToOthers.Count);

			Assert.AreEqual<string>(paymentPlan.DebtsOwedToMe[0].ReasonForDebt,planToCheck.DebtsOwedToMe[0].ReasonForDebt);
			Assert.IsTrue(paymentPlan.DebtsOwedToMe[0].ExpectedEndDate == planToCheck.DebtsOwedToMe[0].ExpectedEndDate);
			Assert.AreEqual<decimal>(paymentPlan.DebtsOwedToMe[0].InitialPayment, planToCheck.DebtsOwedToMe[0].InitialPayment);
			Assert.AreEqual<bool>(paymentPlan.DebtsOwedToMe[0].IsOutstanding, planToCheck.DebtsOwedToMe[0].IsOutstanding);
			Assert.AreEqual<decimal>(paymentPlan.DebtsOwedToMe[0].TotalAmountOwed, planToCheck.DebtsOwedToMe[0].TotalAmountOwed);
		}

		[TestMethod]
		public void ShouldBeAbleToAddPaymentInstallmentToDebt()
		{
			BuildServices();

			var user = SignUpSignInAndGetNewPlan();
			var paymentPlan = _paymentPlanService.GetPaymentPlan(user.Id);

			var emailAddress = string.Format("owes-debt-{0}@integrationtests.com", Guid.NewGuid());
			_signupMgr.SignUpNewUser(emailAddress, "I", "owe", "password");
			var userWhoOwesDebt = _securityService.SignIn(emailAddress, "password");

			paymentPlan.DebtsOwedToMe.Add(new Debt
			{
				ReasonForDebt = "test",
				TotalAmountOwed = 100,
				InitialPayment = 10,
				PaymentPeriod = PaymentPeriod.Weekly,
				StartDate = DateTime.Now,
				UserWhoOwesDebt = userWhoOwesDebt,
			});
			var result = _paymentPlanService.UpdatePaymentPlan(paymentPlan);
			Assert.IsNotNull(result);
			Assert.IsTrue(result.WasSuccessfull);

			var savedPaymentPlan = _paymentPlanService.GetPaymentPlan(user.Id);
			Assert.IsNotNull(savedPaymentPlan.DebtsOwedToMe);
			Assert.IsTrue(savedPaymentPlan.DebtsOwedToMe.Count> 0);

			var installment = new DebtPaymentInstallment();
			installment.TypeOfPayment = PaymentMethodType.Cash;
			var installmentDate = DateTime.Now;
			installment.PaymentDate = installmentDate;
			installment.AmountPaid = 20;
			savedPaymentPlan.DebtsOwedToMe[0].PaymentInstallments.Add(installment);

			result = _paymentPlanService.UpdatePaymentPlan(savedPaymentPlan);
			Assert.IsTrue(result.WasSuccessfull);

			var updatedPlan = _paymentPlanService.GetPaymentPlan(user.Id);
			Assert.IsNotNull(updatedPlan);
			Assert.IsNotNull(updatedPlan.DebtsOwedToMe);
			Assert.IsTrue(updatedPlan.DebtsOwedToMe.Count > 0);
			Assert.IsNotNull(updatedPlan.DebtsOwedToMe[0].PaymentInstallments);
			
			Assert.IsTrue(updatedPlan.DebtsOwedToMe[0].PaymentInstallments.Count > 0);
			Assert.AreNotEqual<Guid>(Guid.Empty,updatedPlan.DebtsOwedToMe[0].PaymentInstallments[0].DebtId);
			Assert.AreEqual<decimal>(20,updatedPlan.DebtsOwedToMe[0].PaymentInstallments[0].AmountPaid);
			Assert.AreEqual<PaymentMethodType>(PaymentMethodType.Cash, updatedPlan.DebtsOwedToMe[0].PaymentInstallments[0].TypeOfPayment);

			Assert.IsTrue(installmentDate.Day == updatedPlan.DebtsOwedToMe[0].PaymentInstallments[0].PaymentDate.Day);
			Assert.IsTrue(installmentDate.Month == updatedPlan.DebtsOwedToMe[0].PaymentInstallments[0].PaymentDate.Month);
			Assert.IsTrue(installmentDate.Year == updatedPlan.DebtsOwedToMe[0].PaymentInstallments[0].PaymentDate.Year);
		}


		private User SignUpSignInAndGetNewPlan()
		{
			var emailAddress = string.Format("{0}@integrationtests.com", Guid.NewGuid());
			var signUpResponse = _signupMgr.SignUpNewUser(emailAddress, "integration", "test", "password");

			Assert.IsNotNull(signUpResponse);
			Assert.IsTrue(signUpResponse.IsSuccessfull);
			Assert.IsNotNull(signUpResponse.AccessGrant);
			Assert.IsFalse(string.IsNullOrWhiteSpace(signUpResponse.AccessGrant.access_token));

			var signInResponse = _securityService.SignIn(emailAddress, "password");
			Assert.IsNotNull(signInResponse);

			var paymentPlan = _paymentPlanService.GetPaymentPlan(signInResponse.Id);
			Assert.IsNotNull(paymentPlan);
			Assert.IsNotNull(paymentPlan.User);
			Assert.IsNotNull(paymentPlan.DebtsOwedToMe);
			Assert.IsNotNull(paymentPlan.DebtsOwedToOthers);

			return signInResponse;
		}

		private void BuildServices()
		{
			var builder = new WebDependencyBuilder();
			var container = builder.BuildDependencies();

			_signupMgr = container.Resolve<ISignupManager>();
			_paymentPlanService = container.Resolve<IPaymentPlanService>();
			_securityService = container.Resolve<IOAuthSecurityService>();
		}
	}
}

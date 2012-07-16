using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.PayMeBack.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Helpers;
using Autofac;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain;
using System.Diagnostics;

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

		[TestMethod]
		public void ShouldNotBeAbleToAddPaymentInstallmentToDebtIfNoMoreDebtOwed()
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
				UserWhoOwesDebt = userWhoOwesDebt
			});
			var result = _paymentPlanService.UpdatePaymentPlan(paymentPlan);
			Assert.IsNotNull(result);
			Assert.IsTrue(result.WasSuccessfull);

			var savedPaymentPlan = _paymentPlanService.GetPaymentPlan(user.Id);
			Assert.IsNotNull(savedPaymentPlan.DebtsOwedToMe);
			Assert.IsTrue(savedPaymentPlan.DebtsOwedToMe.Count > 0);

			// Add installment 1
			var installment1 = new DebtPaymentInstallment();
			installment1.TypeOfPayment = PaymentMethodType.Cash;
			installment1.PaymentDate = DateTime.Now;
			installment1.AmountPaid = 50;
			savedPaymentPlan.DebtsOwedToMe[0].PaymentInstallments.Add(installment1);

			result = _paymentPlanService.UpdatePaymentPlan(savedPaymentPlan);
			Assert.IsTrue(result.WasSuccessfull);

			// Add installment 2 - this should pay it off
			var updatedPlan = _paymentPlanService.GetPaymentPlan(user.Id);
			var installment2 = new DebtPaymentInstallment();
			installment1.TypeOfPayment = PaymentMethodType.Cash;
			installment2.PaymentDate = DateTime.Now;
			installment2.AmountPaid = 40;
			updatedPlan.DebtsOwedToMe[0].PaymentInstallments.Add(installment2);
			result = _paymentPlanService.UpdatePaymentPlan(updatedPlan);
			Assert.IsTrue(result.WasSuccessfull);

			// Add installment 3 - this should fail
			updatedPlan = _paymentPlanService.GetPaymentPlan(user.Id);
			Assert.IsNotNull(updatedPlan);
			Assert.IsNotNull(updatedPlan.DebtsOwedToMe);
			Assert.IsTrue(updatedPlan.DebtsOwedToMe.Count > 0);
			// It should actually be listed asnot outstanding
			Assert.IsFalse(updatedPlan.DebtsOwedToMe[0].IsOutstanding);
			// but lets try and add anyway
			var installment3 = new DebtPaymentInstallment();
			installment3.TypeOfPayment = PaymentMethodType.Cash;
			installment3.PaymentDate = DateTime.Now;
			installment3.AmountPaid = 10;
			updatedPlan.DebtsOwedToMe[0].PaymentInstallments.Add(installment3);

			result = _paymentPlanService.UpdatePaymentPlan(updatedPlan);
			Assert.IsFalse(result.WasSuccessfull);
		}

		private User SignUpSignInAndGetNewPlan()
		{
			var signInResponse = new SignupServiceTests().SignUpThenSignIn();

			Assert.IsNotNull(signInResponse);

			var paymentPlan = _paymentPlanService.GetPaymentPlan(signInResponse.Id);
			Assert.IsNotNull(paymentPlan);
			Assert.IsNotNull(paymentPlan.User);
			Assert.IsNotNull(paymentPlan.DebtsOwedToMe);
			Assert.IsNotNull(paymentPlan.DebtsOwedToOthers);

			return signInResponse;
		}

		[TestMethod]
		public void CanAddDebtOwedToMe()
		{
			BuildServices();

			var user = SignUpSignInAndGetNewPlan();
			var user2 = new SignupServiceTests().SignUpThenSignIn();

			var debtToAdd = new Debt
			{
				ReasonForDebt = "test",
				TotalAmountOwed = 100,
				InitialPayment = 10,
				PaymentPeriod = PaymentPeriod.Weekly,
				StartDate = DateTime.Now,
				UserWhoOwesDebt = new User { Id = user2.Id}
			};
			var addResponse = _paymentPlanService.AddDebtOwed(user.Id, debtToAdd);

			Assert.IsNotNull(addResponse);
			Assert.IsTrue(addResponse.WasSuccessfull);

			var updatedPlan = _paymentPlanService.GetPaymentPlan(user.Id);
			Assert.IsNotNull(updatedPlan);
			Assert.IsNotNull(updatedPlan.DebtsOwedToMe);
			Assert.AreEqual<int>(1,updatedPlan.DebtsOwedToMe.Count);
			Assert.AreEqual<Guid>(user2.Id,updatedPlan.DebtsOwedToMe[0].UserWhoOwesDebt.Id);
		}

		[TestMethod]
		public void CanNotChangeDebtsOwedToOthers()
		{
			BuildServices();

			var user = SignUpSignInAndGetNewPlan();

			var debtToAdd = new Debt
			{
				ReasonForDebt = "test",
				TotalAmountOwed = 100,
				InitialPayment = 10,
				PaymentPeriod = PaymentPeriod.Weekly,
				StartDate = DateTime.Now,
				UserWhoOwesDebt = new User { Id = user.Id }
			};
			var usersPlan = _paymentPlanService.GetPaymentPlan(user.Id);
			Assert.IsNotNull(usersPlan);

			usersPlan.DebtsOwedToOthers.Add(debtToAdd);
			var result = _paymentPlanService.UpdatePaymentPlan(usersPlan);
			Assert.IsNotNull(result);
			Assert.IsTrue(result.WasSuccessfull);

			var planToCheck = _paymentPlanService.GetPaymentPlan(user.Id);
			Assert.IsNotNull(planToCheck);
			Assert.IsNotNull(planToCheck.DebtsOwedToOthers);
			Assert.AreEqual<int>(0,planToCheck.DebtsOwedToOthers.Count);
		}

		[TestMethod]
		public void CanRetrieveDebtsIOweToOthers()
		{
			BuildServices();

			var userWhoOwesDebt = new SignupServiceTests().SignUpThenSignIn();
			var userWhoIsOwedDebt = new SignupServiceTests().SignUpThenSignIn();

			Trace.WriteLine(string.Format("User who owes debt = {0}",userWhoOwesDebt.Id));
			Trace.WriteLine(string.Format("User who is owed the debt = {0}", userWhoIsOwedDebt.Id));

			var debtToAdd = new Debt
			{
				ReasonForDebt = "test",
				TotalAmountOwed = 100,
				InitialPayment = 10,
				PaymentPeriod = PaymentPeriod.Weekly,
				StartDate = DateTime.Now,
				UserWhoOwesDebt = new User { Id = userWhoOwesDebt.Id }
			};
			var addResponse = _paymentPlanService.AddDebtOwed(userWhoIsOwedDebt.Id, debtToAdd);

			Assert.IsNotNull(addResponse);
			Assert.IsTrue(addResponse.WasSuccessfull);

			var paymentPlan = _paymentPlanService.GetPaymentPlan(userWhoOwesDebt.Id);
			Assert.IsNotNull(paymentPlan);
			Assert.IsNotNull(paymentPlan.DebtsOwedToOthers);
			Assert.AreEqual<int>(1, paymentPlan.DebtsOwedToOthers.Count);
			Assert.AreEqual<Guid>(userWhoOwesDebt.Id, paymentPlan.DebtsOwedToOthers[0].UserWhoOwesDebt.Id);
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

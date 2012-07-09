using System;
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
			Assert.IsTrue(paymentPlan.DateCreated == planToCheck.DateCreated);
			Assert.AreEqual<int>(paymentPlan.DebtsOwedToMe.Count,planToCheck.DebtsOwedToMe.Count);
			Assert.AreEqual<int>(paymentPlan.DebtsOwedToOthers.Count, planToCheck.DebtsOwedToOthers.Count);

			Assert.AreEqual<string>(paymentPlan.DebtsOwedToMe[0].ReasonForDebt,planToCheck.DebtsOwedToMe[0].ReasonForDebt);
			Assert.IsTrue(paymentPlan.DebtsOwedToMe[0].ExpectedEndDate == planToCheck.DebtsOwedToMe[0].ExpectedEndDate);
			Assert.AreEqual<decimal>(paymentPlan.DebtsOwedToMe[0].InitialPayment, planToCheck.DebtsOwedToMe[0].InitialPayment);
			Assert.AreEqual<bool>(paymentPlan.DebtsOwedToMe[0].IsOutstanding, planToCheck.DebtsOwedToMe[0].IsOutstanding);
			Assert.AreEqual<decimal>(paymentPlan.DebtsOwedToMe[0].TotalAmountOwed, planToCheck.DebtsOwedToMe[0].TotalAmountOwed);
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

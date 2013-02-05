using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Web.Helpers;
using Autofac;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain;
using System.Diagnostics;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.PayMeBack.IntegrationTests.ServiceTests;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.IntegrationTests.ServiceTests
{
    [TestClass]
    public class NotificationServiceTests
    {
        private ISignupManager _signupMgr;
        private IOAuthSecurityService _securityService;
        private IUserEngine _userEngine;
        private INotificationService _notificationService;
        private IPaymentPlanService _paymentPlanService;

        [TestMethod]
        public void ShouldBeAbleToGetNotificationOptionsForValidUser()
        {
            BuildServices();
            var signInTests = new SignupServiceTests();
            var user = signInTests.SignUpThenSignIn();

            // Add debt
            var paymentPlan = _paymentPlanService.GetPaymentPlan(user.Id);

            var emailAddress1 = string.Format("owes-debt-{0}@integrationtests.com", Guid.NewGuid());

            paymentPlan.DebtsOwedToMe.Add(new Debt
            {
                ReasonForDebt = "test",
                TotalAmountOwed = 100,
                InitialPayment = 10,
                PaymentPeriod = PaymentPeriod.Weekly,
                StartDate = DateTime.Now,
                UserWhoOwesDebt = new User { EmailAddress = emailAddress1 }
            });
            var result = _paymentPlanService.UpdatePaymentPlan(paymentPlan);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.WasSuccessfull);
            var updatedPlan = _paymentPlanService.GetPaymentPlan(user.Id);
            var debtId = updatedPlan.DebtsOwedToMe[0].Id;

            var options = _notificationService.GetNotificationOptionsForUserDebt(user.Id, debtId);

            Assert.IsNotNull(options);
            Assert.AreEqual<Guid>(user.Id, options.UserId);
            Assert.AreEqual<Guid>(debtId, options.DebtId);
        }
        private void BuildServices()
        {
            var builder = new WebDependencyBuilder();
            var container = builder.BuildDependencies();

            _signupMgr = container.Resolve<ISignupManager>();
            _securityService = container.Resolve<IOAuthSecurityService>();
            _userEngine = container.Resolve<IUserEngine>();
            _notificationService = container.Resolve<INotificationService>();
            _paymentPlanService = container.Resolve<IPaymentPlanService>();

        }
    }
}

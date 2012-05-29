using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Data=Glav.PayMeBack.Web.Data;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Domain.Engines;

namespace PayMeBackWeb.UnitTests.Services
{
	[TestClass]
	public class PaymentPlanServiceTests
	{
		private Mock<IUserEngine> _userEngine;
		private IPaymentPlanService _paymentPlanService;
		private Mock<Data.ICrudRepository> _crudRepo;
		private Mock<Data.IDebtRepository> _debtRepo;

		[TestMethod]
		public void ShouldBeAbleToCreateANewPaymentPlanForAUserIfNoPlanExists()
		{
			var testUser = new User { EmailAddress = "newuser@test.com", Id = Guid.NewGuid(), FirstNames = "test", Surname = "user" };
			_userEngine.Setup<User>(m => m.GetUserById(It.IsAny<Guid>())).Returns(testUser);

			var plan = _paymentPlanService.GetPaymentPlan(testUser.Id);

			Assert.IsNotNull(plan);
			Assert.IsNotNull(plan.User);
			Assert.IsNotNull(plan.DebtsOwedToMe);
			Assert.IsNotNull(plan.DebtsOwedToOthers);
			Assert.AreEqual<int>(0, plan.DebtsOwedToMe.Count);
			Assert.AreEqual<int>(0, plan.DebtsOwedToOthers.Count);
			Assert.AreEqual<Guid>(testUser.Id, plan.User.Id);
		}

		[TestMethod]
		public void ShouldBeAleToAddDebtToPaymentPlan()
		{
			var testUser = new User { EmailAddress = "test@test.com", Id = Guid.NewGuid(), FirstNames = "test", Surname = "user"};
			_userEngine.Setup<User>(m => m.GetUserById(It.IsAny<Guid>())).Returns(testUser);
			

			var debt = new Debt { Id = Guid.NewGuid(), Notes = "test debt", ReasonForDebt = "drugs", TotalAmountOwed = 100};
			_paymentPlanService.AddDebt(testUser.Id, debt);
		}

		[TestInitialize]
		public void Initialise()
		{
			_crudRepo = new Mock<Data.ICrudRepository>();
			_userEngine = new Mock<IUserEngine>();
			_debtRepo = new Mock<Data.IDebtRepository>();
			_paymentPlanService = new PaymentPlanService(_userEngine.Object, _crudRepo.Object, _debtRepo.Object);
		}
	}
}

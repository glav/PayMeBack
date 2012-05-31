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
using System.Linq.Expressions;
using Glav.CacheAdapter.Core;

namespace PayMeBackWeb.UnitTests.Services
{
	[TestClass]
	public class PaymentPlanServiceTests
	{
		private Mock<IUserEngine> _userEngine;
		private IPaymentPlanService _paymentPlanService;
		private Mock<Data.ICrudRepository> _crudRepo;
		private Mock<Data.IDebtRepository> _debtRepo;
		private Mock<ICacheProvider> _cacheProvider;

		[TestMethod]
		public void ShouldBeAbleToCreateANewPaymentPlanForAUserIfNoPlanExists()
		{
			var testDetailUser = new Data.UserDetail {EmailAddress = "newuser@test.com", Id = Guid.NewGuid(), FirstNames = "test", Surname = "user" };
			var testUser = new User(testDetailUser);
			_userEngine.Setup<User>(m => m.GetUserById(It.IsAny<Guid>())).Returns(testUser);
			_crudRepo.Setup<Data.UserDetail>(m => m.GetSingle<Data.UserDetail>(It.IsAny<Expression<Func<Data.UserDetail,bool>>>())).Returns(testDetailUser);
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
			var testDetailUser = new Data.UserDetail { EmailAddress = "test@test.com", Id = Guid.NewGuid(), FirstNames = "test", Surname = "user" };
			var testUser = new User(testDetailUser);
			_userEngine.Setup<User>(m => m.GetUserById(It.IsAny<Guid>())).Returns(testUser);
			_crudRepo.Setup<Data.UserDetail>(m => m.GetSingle<Data.UserDetail>(It.IsAny<Expression<Func<Data.UserDetail, bool>>>())).Returns(testDetailUser);

			_debtRepo.Setup<Data.UserPaymentPlan>(m => m.GetUserPaymentPlan(testDetailUser.Id)).Returns(new Data.UserPaymentPlan { UserDetail= testDetailUser, UserId = testUser.Id });
			_debtRepo.Setup(m => m.UpdateUserPaymentPlan(It.IsAny<Data.UserPaymentPlan>()));

			var debt = new Debt { Id = Guid.Empty, Notes = "test debt", ReasonForDebt = "drugs", TotalAmountOwed = 100};
			_paymentPlanService.AddDebtOwed(testUser.Id, debt);
		}

		[TestInitialize]
		public void Initialise()
		{
			_crudRepo = new Mock<Data.ICrudRepository>();
			_userEngine = new Mock<IUserEngine>();
			_debtRepo = new Mock<Data.IDebtRepository>();
			_cacheProvider = new Mock<ICacheProvider>();

			_paymentPlanService = new PaymentPlanService(_userEngine.Object, _crudRepo.Object, _debtRepo.Object, _cacheProvider.Object);
		}
	}
}

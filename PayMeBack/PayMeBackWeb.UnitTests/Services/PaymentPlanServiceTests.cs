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
		public void ShouldBeAbleToAddDebtToPaymentPlan()
		{
			var testDetailUser = new Data.UserDetail { EmailAddress = "test@test.com", Id = Guid.NewGuid(), FirstNames = "test", Surname = "user" };
			var testUser = new User(testDetailUser);
			_userEngine.Setup<User>(m => m.GetUserById(It.IsAny<Guid>())).Returns(testUser);
			_crudRepo.Setup<Data.UserDetail>(m => m.GetSingle<Data.UserDetail>(It.IsAny<Expression<Func<Data.UserDetail, bool>>>())).Returns(testDetailUser);

			_debtRepo.Setup<Data.UserPaymentPlanDetail>(m => m.GetUserPaymentPlan(testDetailUser.Id)).Returns(new Data.UserPaymentPlanDetail { UserDetail= testDetailUser, UserId = testUser.Id });
			_debtRepo.Setup(m => m.UpdateUserPaymentPlan(It.IsAny<Data.UserPaymentPlanDetail>()));

			var debt = new Debt { Id = Guid.Empty, Notes = "test debt", ReasonForDebt = "drugs", TotalAmountOwed = 100};
			var result = _paymentPlanService.AddDebtOwed(testUser.Id, debt);

			Assert.IsNotNull(result);
			Assert.IsTrue(result.WasSuccessfull);
		}

		[TestMethod]
		public void ShouldBeAbleToAddPaymentInstallmentToDebtInPaymentPlan()
		{
			var testDetailUser = new Data.UserDetail { EmailAddress = "test@test.com", Id = Guid.NewGuid(), FirstNames = "test", Surname = "user" };
			var testUser = new User(testDetailUser);
			var userDetailWhoOwesDebt = new Data.UserDetail { EmailAddress = "iowe@test.com", Id = Guid.NewGuid(), FirstNames = "I", Surname = "Owe" };
			var userWhoOwesDebt = new User(userDetailWhoOwesDebt);

			_userEngine.Setup<User>(m => m.GetUserById(testDetailUser.Id)).Returns(testUser);
			_userEngine.Setup<User>(m => m.GetUserById(userDetailWhoOwesDebt.Id)).Returns(userWhoOwesDebt);
			
			_crudRepo.Setup<Data.UserDetail>(m => m.GetSingle<Data.UserDetail>(u => u.Id == testDetailUser.Id)).Returns(testDetailUser);
			_crudRepo.Setup<Data.UserDetail>(m => m.GetSingle<Data.UserDetail>(u => u.Id == userDetailWhoOwesDebt.Id)).Returns(userDetailWhoOwesDebt);

			var cacheKey = string.Format("UserPaymentPlan_{0}",testDetailUser.Id);

			var paymentPlanInDb = new Data.UserPaymentPlanDetail { UserDetail = testDetailUser, UserId = testUser.Id, Id = Guid.NewGuid() };
			paymentPlanInDb.DebtDetails = new List<Glav.PayMeBack.Web.Data.DebtDetail>();
			paymentPlanInDb.DebtDetails.Add(new Data.DebtDetail
				{ 
					Id= Guid.NewGuid(), 
					DateCreated = DateTime.Now.AddDays(-1), 
					TotalAmountOwed=100,
					UserDetail = testDetailUser,
					StartDate = DateTime.Now.AddDays(-1),
					IsOutstanding = true,
					UserIdWhoOwesDebt= userDetailWhoOwesDebt.Id
				});
			_cacheProvider.Setup<Data.UserPaymentPlanDetail>(m => m.Get<Data.UserPaymentPlanDetail>(cacheKey, It.IsAny<DateTime>(), It.IsAny<Func<Data.UserPaymentPlanDetail>>())).Returns(paymentPlanInDb);
			_debtRepo.Setup<Data.UserPaymentPlanDetail>(m => m.GetUserPaymentPlan(testDetailUser.Id)).Returns(paymentPlanInDb);
			_debtRepo.Setup(m => m.UpdateUserPaymentPlan(It.IsAny<Data.UserPaymentPlanDetail>()));

			//Now add in a payment
			var paymentPlan = _paymentPlanService.GetPaymentPlan(testUser.Id);
			Assert.IsNotNull(paymentPlan);
			Assert.IsNotNull(paymentPlan.DebtsOwedToMe);
			Assert.IsNotNull(paymentPlan.DebtsOwedToOthers);
			Assert.IsTrue(paymentPlan.DebtsOwedToMe.Count > 0);
			Assert.IsNotNull(paymentPlan.DebtsOwedToMe[0].PaymentInstallments);

			paymentPlan.DebtsOwedToMe[0].PaymentInstallments.Add(new DebtPaymentInstallment { AmountPaid = 5, PaymentDate = DateTime.UtcNow, TypeOfPayment = PaymentMethodType.Cash });
			var result = _paymentPlanService.UpdatePaymentPlan(paymentPlan);

			Assert.IsNotNull(result);
			Assert.IsTrue(result.WasSuccessfull);
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

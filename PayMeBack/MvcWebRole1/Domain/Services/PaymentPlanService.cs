using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class PaymentPlanService : IPaymentPlanService
	{
		private IUserService _userService;
		private IUserRepository _repository;

		public PaymentPlanService(IUserService userService, IUserRepository repository)
		{
			_userService = userService;
			_repository = repository;
		}

		public UserPaymentPlan GetPaymentPlan(Guid userId)
		{
			var paymentPlan = new UserPaymentPlan();
			paymentPlan.User = new User(_repository.GetUser(userId));

			//TODO: Get plans from repository
			paymentPlan.DebtsOwedToMe = new List<DebtPaymentPlan>();
			paymentPlan.DebtsOwedToOthers = new List<DebtPaymentPlan>();
			return paymentPlan;
		}

		public void AddDebt(Guid userId, Debt debt)
		{
			throw new NotImplementedException();
		}

		public void UpdateDebt(Guid userId, Debt debt)
		{
			throw new NotImplementedException();
		}

		public void RemoveDebt(Guid userId, Guid debtId)
		{
			throw new NotImplementedException();
		}
	}
}
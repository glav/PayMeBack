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
		private ICrudRepository _crudRepository;

		public PaymentPlanService(IUserService userService, ICrudRepository crudRepository)
		{
			_userService = userService;
			_crudRepository = crudRepository;
		}

		public UserPaymentPlan GetPaymentPlan(Guid userId)
		{
			var paymentPlan = new UserPaymentPlan();
			paymentPlan.User = new User(_crudRepository.GetSingle<UserDetail>(u => u.Id == userId));

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
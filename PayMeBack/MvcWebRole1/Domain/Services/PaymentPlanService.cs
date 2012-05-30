using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data = Glav.PayMeBack.Web.Data;
using Glav.PayMeBack.Web.Domain.Engines;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class PaymentPlanService : IPaymentPlanService
	{
		private IUserEngine _userEngine;
		private Data.ICrudRepository _crudRepository;
		private Data.IDebtRepository _debtRepository;

		public PaymentPlanService(IUserEngine userEngine, Data.ICrudRepository crudRepository, Data.IDebtRepository debtRepository)
		{
			_userEngine = userEngine;
			_crudRepository = crudRepository;
			_debtRepository = debtRepository;
		}

		public UserPaymentPlan GetPaymentPlan(Guid userId)
		{
			var paymentPlanDetail = _debtRepository.GetUserPaymentPlan(userId);

			var paymentPlan = new UserPaymentPlan();
			if (paymentPlanDetail == null || paymentPlanDetail.Id == Guid.Empty)
			{
				paymentPlan.User = new User(_crudRepository.GetSingle<Data.UserDetail>(u => u.Id == userId));
				paymentPlan.DebtsOwedToMe = new List<DebtPaymentPlan>();
				paymentPlan.DebtsOwedToOthers = new List<DebtPaymentPlan>();
				paymentPlan.Id = Guid.Empty;
				return paymentPlan;
			}

			paymentPlan.Id = paymentPlanDetail.Id;
			paymentPlan.User = new Domain.User(paymentPlanDetail.UserDetail);

			paymentPlan.DebtsOwedToMe = GetDebtsRelatedToUser(userId, paymentPlanDetail, true);
			paymentPlan.DebtsOwedToOthers = GetDebtsRelatedToUser(userId, paymentPlanDetail, false);
			return paymentPlan;
		}

		private List<DebtPaymentPlan> GetDebtsRelatedToUser(Guid userId, Data.UserPaymentPlan paymentPlanDetail, bool debtsOwedToUser)
		{
			var debtPaymentPlans = new List<DebtPaymentPlan>();
			if (paymentPlanDetail.DebtPaymentPlans != null && paymentPlanDetail.DebtPaymentPlans.Count > 0)
			{
				paymentPlanDetail.DebtPaymentPlans.ToList().ForEach(p =>
				{
					if ((p.UserIdWhoOwesDebt != userId && debtsOwedToUser) || (p.UserIdWhoOwesDebt == userId && !debtsOwedToUser))
					{
						debtPaymentPlans.Add(new DebtPaymentPlan(p));
					}
				});
			}

			return debtPaymentPlans;
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
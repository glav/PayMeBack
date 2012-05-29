using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data=Glav.PayMeBack.Web.Data;
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
			var paymentPlanDetail = _crudRepository.GetSingle<Data.UserPaymentPlan>(p => p.Id == userId);
			var paymentPlan = new UserPaymentPlan();
			if (paymentPlanDetail == null)
			{
				paymentPlan = new UserPaymentPlan
				{
					DebtsOwedToMe = new List<DebtPaymentPlan>(),
					DebtsOwedToOthers = new List<DebtPaymentPlan>(),
					Id = Guid.Empty
				};
			}
			paymentPlan.User = _userEngine.GetUserById(userId);

			if (paymentPlanDetail.Id !=Guid.Empty)
			{
				paymentPlan.Id = paymentPlanDetail.Id;
				paymentPlan.DebtsOwedToMe = GetDebtPaymentPlansOwedToUser();
				paymentPlan.DebtsOwedToOthers = GetDebtPaymentPlansThatUserOwes();
			}
			return paymentPlan;
		}

		private List<DebtPaymentPlan> GetDebtPaymentPlansThatUserOwes()
		{
			throw new NotImplementedException();
		}

		private List<DebtPaymentPlan> GetDebtPaymentPlansOwedToUser()
		{
			throw new NotImplementedException();
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
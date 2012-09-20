using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	public interface IDebtRepository
	{
		IEnumerable<DebtDetail> GetAllPaymentPlansForUser(Guid userPaymentPlanId);
		DebtDetail GetDebt(Guid debtId);
		void UpdateDebt(DebtDetail debt);
		void DeleteDebt(Guid debtId);

		UserPaymentPlanDetail GetUserPaymentPlan(Guid userId);
		void UpdateUserPaymentPlan(UserPaymentPlanDetail userPaymentPlan);
	}
}
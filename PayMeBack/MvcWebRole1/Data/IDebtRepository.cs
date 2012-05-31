using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	public interface IDebtRepository
	{
		IEnumerable<Debt> GetAllPaymentPlansForUser(Guid userPaymentPlanId);
		Debt GetDebt(Guid debtId);
		void UpdateDebt(Debt debt);
		void DeleteDebt(Guid debtId);

		UserPaymentPlan GetUserPaymentPlan(Guid userId);
		void UpdateUserPaymentPlan(UserPaymentPlan userPaymentPlan);
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	public interface IDebtRepository
	{
		IEnumerable<DebtPaymentPlan> GetAllPaymentPlansForUser(Guid userPaymentPlanId);
		Debt GetDebt(Guid debtId);
		void UpdateDebt(Debt debt);
		void DeleteDebt(Guid debtId);
		DebtPaymentPlan GetPaymentPlan(Guid paymentPlanId);
		void UpdatePaymentPlan(DebtPaymentPlan paymentPlan);
		void DeletePaymentPlan(Guid paymentPlanId);

		UserPaymentPlan GetUserPaymentPlan(Guid userId);
		void UpdateUserPaymentPlan(UserPaymentPlan userPaymentPlan);
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface IPaymentPlanService
	{
		UserPaymentPlan GetPaymentPlan(Guid userId);
		void AddDebtOwed(Guid userId, Debt debt);
		void AddDebtOwing(Guid userId, Debt debt);
		void UpdatePaymentPlan(UserPaymentPlan usersPaymentPlan);
		void RemoveDebt(Guid userId, DebtPaymentPlan debtPaymentPlan);
	}
}
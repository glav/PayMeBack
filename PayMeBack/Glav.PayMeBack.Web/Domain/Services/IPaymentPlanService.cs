using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface IPaymentPlanService
	{
		UserPaymentPlan GetPaymentPlan(Guid userId);
		DataAccessResult AddDebtOwed(Guid userId, Debt debt);
		DataAccessResult UpdatePaymentPlan(UserPaymentPlan usersPaymentPlan);
		DataAccessResult RemoveDebt(Guid userId, Guid debtId);
		DebtSummary GetDebtSummaryForUser(Guid userId);
        DataAccessResult AddPaymentInstallmentToPlan(Guid userId, DebtPaymentInstallment installment);
        UserPaymentPlan GetOutstandingDebtsPaymentPlan(Guid userId);
	}
}
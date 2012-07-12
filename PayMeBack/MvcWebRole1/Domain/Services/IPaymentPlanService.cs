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
		DataAccessResult AddDebtOwing(Guid userId, Debt debt);
		DataAccessResult UpdatePaymentPlan(UserPaymentPlan usersPaymentPlan);
		void RemoveDebt(Guid userId, Debt debtPaymentPlan);
	}
}
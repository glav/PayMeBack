using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface IPaymentPlanService
	{
		UserPaymentPlan GetPaymentPlan(Guid userId);
		void AddDebt(Guid userId, Debt debt);
		void UpdateDebt(Guid userId, Debt debt);
		void RemoveDebt(Guid userId, Guid debtId);
	}
}
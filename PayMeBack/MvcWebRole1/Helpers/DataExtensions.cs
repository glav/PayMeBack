using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain=Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Helpers
{
	public static class DataExtensions
	{
		public static UserDetail ToDataRecord(this Domain.User user)
		{
			var detail = new UserDetail
			{
				Id = user.Id,
				EmailAddress = user.EmailAddress,
				FirstNames = user.FirstNames,
				Surname = user.Surname,
			};
			return detail;
		}

		public static Debt ToDataRecord(this Domain.Debt debt)
		{
			var detail = new Debt
			{
				Id = debt.Id,
				Notes = debt.Notes,
				ReasonForDebt = debt.ReasonForDebt
			};
			return detail;
		}
		public static DebtPaymentPlan ToDataRecord(this Domain.DebtPaymentPlan debtPlan)
		{
			var detail = new DebtPaymentPlan
			{
				Id = debtPlan.Id,
				Debt = debtPlan.DebtOwed.ToDataRecord(),
				DebtId = debtPlan.DebtOwed.Id,
				ExpectedEndDate = debtPlan.ExpectedEndDate,
				InitialPayment = debtPlan.InitialPayment,
				IsOutstanding = debtPlan.IsOutstanding,
				PaymentPeriod = (int)debtPlan.PaymentPeriod,
				StartDate = debtPlan.StartDate,
				UserIdWhoOwesDebt = debtPlan.UserWhoOwesDebt.Id,
				UserDetail = debtPlan.UserWhoOwesDebt.ToDataRecord()
			};
			return detail;
		}
	}
}
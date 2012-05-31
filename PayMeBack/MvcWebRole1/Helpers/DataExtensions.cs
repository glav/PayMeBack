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
				ReasonForDebt = debt.ReasonForDebt,
				ExpectedEndDate = debt.ExpectedEndDate,
				InitialPayment = debt.InitialPayment,
				IsOutstanding = debt.IsOutstanding,
				PaymentPeriod = (int)debt.PaymentPeriod,
				StartDate = debt.StartDate,
				UserIdWhoOwesDebt = debt.UserWhoOwesDebt.Id,
				UserDetail = debt.UserWhoOwesDebt.ToDataRecord()
			};
			return detail;
		}
	}
}
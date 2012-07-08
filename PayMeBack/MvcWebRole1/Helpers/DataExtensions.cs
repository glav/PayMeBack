using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;
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

		public static DebtDetail ToDataRecord(this Domain.Debt debt)
		{
			var detail = new DebtDetail
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
				UserDetail = debt.UserWhoOwesDebt.ToDataRecord(),
				TotalAmountOwed = debt.TotalAmountOwed
			};
			return detail;
		}

		public static DebtPaymentInstallmentDetail ToDataRecord(this DebtPaymentInstallment paymentInstallment)
		{
			var detail = new DebtPaymentInstallmentDetail
			             	{
								Id = paymentInstallment.Id,
								AmountPaid = paymentInstallment.AmountPaid,
								DebtId = paymentInstallment.DebtId,
								PaymentDate = paymentInstallment.PaymentDate,
								PaymentMethod = (int)paymentInstallment.TypeOfPayment
			             	};
			return detail;
		}
	}
}
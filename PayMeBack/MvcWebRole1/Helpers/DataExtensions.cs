using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Domain;
using Domain=Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Data;
using Glav.PayMeBack.Web.Models;

namespace Glav.PayMeBack.Web.Helpers
{
	public static class DataExtensions
	{
		public static UserDetail ToDataRecord(this User user)
		{
			var detail = new UserDetail
			{
				Id = user.Id,
				EmailAddress = user.EmailAddress,
				FirstNames = user.FirstNames,
				Surname = user.Surname
            };
			return detail;
		}

		public static DebtDetail ToDataRecord(this Debt debt)
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
				TotalAmountOwed = debt.TotalAmountOwed,
				DateCreated = debt.DateCreated.GetValueOrDefault()
			};
			if (detail.DateCreated == DateTime.MinValue)
			{
				detail.DateCreated = DateTime.UtcNow;
			}

			if (debt.PaymentInstallments != null && debt.PaymentInstallments.Count > 0)
			{
				detail.DebtPaymentInstallmentDetails = new List<DebtPaymentInstallmentDetail>();
				debt.PaymentInstallments.ForEach(i=>
				                                     {
				                                         var dataRecord = i.ToDataRecord();
				                                         dataRecord.DebtId = i.DebtId;
				                                         detail.DebtPaymentInstallmentDetails.Add(dataRecord);
				                                     });
			}
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

		public static UserPaymentPlanDetail ToDataRecord(this UserPaymentPlan paymentPlan)
		{
			var detail = new UserPaymentPlanDetail
			             	{
								DateCreated = paymentPlan.DateCreated,
								Id = paymentPlan.Id,

								UserId = paymentPlan.User != null ? paymentPlan.User.Id : Guid.Empty
			             	};
            if (paymentPlan.User != null)
            {
                detail.UserDetail = paymentPlan.User.ToDataRecord();
            }
			detail.DebtDetails = new List<DebtDetail>();
			// Only do the debts owed to the person. The user cannot change who they owe
			// to and how much they owe, just who owes them
			if (paymentPlan.DebtsOwedToMe != null)
			{
				paymentPlan.DebtsOwedToMe.ForEach(d =>
				                                      {
				                                          var dataRecord = d.ToDataRecord();
                                                          if (paymentPlan.Id != Guid.Empty)
                                                          {
                                                              dataRecord.UserPaymentPlanId = paymentPlan.Id;
                                                          }
				                                          detail.DebtDetails.Add(dataRecord);

				                                      });
			}
			return detail;
		}

	}
}
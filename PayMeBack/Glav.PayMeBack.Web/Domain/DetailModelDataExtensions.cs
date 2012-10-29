using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain
{
	public static class DetailModelDataExtensions
	{
		public static UserPaymentPlan ToModel(this UserPaymentPlanDetail detail)
		{
			var plan = new UserPaymentPlan();
			if (detail.UserDetail != null)
			{
				plan.User = detail.UserDetail.ToModel();
			}
			else
			{
				plan.User = new User { Id = detail.UserId };
			}
			plan.DateCreated = detail.DateCreated;
			return plan;
		}

		public static DebtPaymentInstallment ToModel(this DebtPaymentInstallmentDetail detail)
		{
			var installment = new DebtPaymentInstallment();
			installment.Id = detail.Id;
			installment.DebtId = detail.DebtId;
			installment.PaymentDate = detail.PaymentDate;
			installment.AmountPaid = detail.AmountPaid;
			installment.TypeOfPayment = (PaymentMethodType)detail.PaymentMethod;
			return installment;
		}

		public static User ToModel(this UserDetail detail)
		{
			var user = new User();
			user.Id = detail.Id;
			user.EmailAddress = detail.EmailAddress;
			user.FirstNames = detail.FirstNames;
			user.Surname = detail.Surname;
			return user;
		}

		public static Debt ToModel(this DebtDetail detail)
		{
			var debt = new Debt();
						debt.Id = detail.Id;
			if (detail.UserDetail != null)
			{
				debt.UserWhoOwesDebt = detail.UserDetail.ToModel();
			}
			else
			{
				debt.UserWhoOwesDebt = new User { Id = detail.UserIdWhoOwesDebt };
			}
			debt.PaymentPeriod = (PaymentPeriod)detail.PaymentPeriod;
			debt.StartDate = detail.StartDate;
			debt.ExpectedEndDate = detail.ExpectedEndDate;
			debt.InitialPayment = detail.InitialPayment.HasValue ? detail.InitialPayment.Value : 0;
			debt.IsOutstanding = detail.IsOutstanding.HasValue ? detail.IsOutstanding.Value : true;
			if (detail.UserDetail != null)
			{
				debt.UserWhoOwesDebt = detail.UserDetail.ToModel();
			}
			debt.TotalAmountOwed = detail.TotalAmountOwed;
			debt.ReasonForDebt = detail.ReasonForDebt;
			debt.Notes = detail.Notes;
			debt.PaymentInstallments = new List<DebtPaymentInstallment>();
			if (detail.DebtPaymentInstallmentDetails != null)
			{
				detail.DebtPaymentInstallmentDetails.OrderBy(i => i.PaymentDate).ToList().ForEach(d => debt.PaymentInstallments.Add(d.ToModel()));
			}
			debt.DateCreated = detail.DateCreated;

			return debt;

		}
	}
}
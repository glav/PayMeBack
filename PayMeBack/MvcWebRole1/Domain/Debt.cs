using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public class Debt : BaseModel
	{
		public Debt() { }

		public Debt(Data.Debt debtDetail)
		{
			if (debtDetail.UserDetail != null)
			{
				UserWhoOwesDebt = new User(debtDetail.UserDetail);
			}
			else
			{
				UserWhoOwesDebt = new User { Id = debtDetail.UserIdWhoOwesDebt };
			}
			PaymentPeriod = (Domain.PaymentPeriod)debtDetail.PaymentPeriod;
			StartDate = debtDetail.StartDate;
			ExpectedEndDate = debtDetail.ExpectedEndDate;
			InitialPayment = debtDetail.InitialPayment.HasValue ? debtDetail.InitialPayment.Value : 0;
			IsOutstanding = debtDetail.IsOutstanding.HasValue ? debtDetail.IsOutstanding.Value : true;
			if (debtDetail.UserDetail != null)
			{
				UserWhoOwesDebt = new User(debtDetail.UserDetail);
			}
			TotalAmountOwed = debtDetail.TotalAmountOwed;
			ReasonForDebt = debtDetail.ReasonForDebt;
			Notes = debtDetail.Notes;
		}

		public User UserWhoOwesDebt { get; set; }
		public PaymentPeriod PaymentPeriod { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? ExpectedEndDate { get; set; }
		public decimal InitialPayment { get; set; }  // How much have they paid off when the debt started?
		public List<DebtPaymentInstallment> PaymentInstallments { get; set; }
		public bool IsOutstanding { get; set; }  // Has it been paid off yet?

		public decimal TotalAmountOwed { get; set; }
		public string ReasonForDebt { get; set; }
		public string Notes { get; set; }

	}
}
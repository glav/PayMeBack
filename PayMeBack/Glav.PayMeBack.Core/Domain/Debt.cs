using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Core.Domain
{
	public class Debt : BaseModel
	{
		public Debt()
		{
			IsOutstanding = true;
		}

		public User UserWhoOwesDebt { get; set; }
		public PaymentPeriod PaymentPeriod { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? ExpectedEndDate { get; set; }
		public DateTime? DateCreated { get; set; }
		public decimal InitialPayment { get; set; }  // How much have they paid off when the debt started?
		public List<DebtPaymentInstallment> PaymentInstallments { get; set; }
		public bool IsOutstanding { get; set; }  // Has it been paid off yet?

		public decimal TotalAmountOwed { get; set; }
		public string ReasonForDebt { get; set; }
		public string Notes { get; set; }

		public decimal AmountLeftOwing()
		{
			var amountRemaining = TotalAmountOwed - InitialPayment;
			if (PaymentInstallments != null)
			{
				PaymentInstallments.ForEach(p =>
				                            	{
				                            		amountRemaining -= p.AmountPaid;
				                            	});	
			}
			return amountRemaining;
		}

		public decimal LastAmountPaid()
		{
			if (PaymentInstallments != null && PaymentInstallments.Count > 0)
			{
				return PaymentInstallments[PaymentInstallments.Count - 1].AmountPaid;
			}
			return InitialPayment;
		}
		public DateTime? LastPaymentDate()
		{
			if (PaymentInstallments != null && PaymentInstallments.Count > 0)
			{
				return PaymentInstallments[PaymentInstallments.Count - 1].PaymentDate;
			}
			return null;
		}

	}
}
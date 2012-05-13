using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public class DebtPaymentPlan : BaseModel
	{
		public User UserWhoOwesDebt { get; set; }
		public Debt DebtOwed { get; set; }
		public PaymentPeriod PaymentPeriod { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? ExpectedEndDate { get; set; }
		public decimal InitialPayment { get; set; }  // How much have they paid off when the debt started?
		public List<DebtPaymentInstallment> PaymentInstallments { get; set; }
		public bool IsOutstanding { get; set; }  // Has it been paid off yet?
	}
}
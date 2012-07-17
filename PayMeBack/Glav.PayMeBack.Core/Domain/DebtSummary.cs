using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.PayMeBack.Core.Domain
{
	public class DebtSummary
	{
		public DebtSummary()
		{
			DebtsOwedToYou = new List<DebtSummaryItem>();
			DebtsYouOwe = new List<DebtSummaryItem>();
			TotalAmountOwedToYou = 0;
			TotalAmountYouOwe = 0;
		}
		public decimal TotalAmountOwedToYou { get; set; }
		public decimal TotalAmountYouOwe { get; set; }
		public List<DebtSummaryItem> DebtsOwedToYou { get; set; }
		public List<DebtSummaryItem> DebtsYouOwe { get; set; }
	}

	public class DebtSummaryItem
	{
		public DateTime StartDate { get; set; }
		public decimal AmountOwing { get; set; }
		public DateTime? LastPaymentDate { get; set; }
		public decimal LastAmountPaid { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public class Debt : BaseModel
	{
		public decimal TotalAmountOwed { get; set; }
		public PaymentPeriod PaymentPeriod { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? ExpectedEndDate { get; set; }
		public decimal InitialPayment { get; set; }  // How much have they paid off when the debt started?
	}
}
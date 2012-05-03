using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public class DebtPaymentInstallment : BaseModel
	{
		public DateTime PaymentDate { get; set; }
		public decimal AmountPaid { get; set; }
		public PaymentMethodType TypeOfPayment { get; set; }
	}
}
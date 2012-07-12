using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Core.Domain
{
	public class DebtPaymentInstallment : BaseModel
	{
		public Guid DebtId { get; set; }
		public DateTime PaymentDate { get; set; }
		public decimal AmountPaid { get; set; }
		public PaymentMethodType TypeOfPayment { get; set; }

	}

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain
{
	public class DebtPaymentInstallment : BaseModel
	{
		public DebtPaymentInstallment() {}

		public DebtPaymentInstallment(DebtPaymentInstallmentDetail detailRecord)
		{
			Id = detailRecord.Id;
			DebtId = detailRecord.DebtId;
			PaymentDate = detailRecord.PaymentDate;
			AmountPaid = detailRecord.AmountPaid;
			TypeOfPayment = (PaymentMethodType) detailRecord.PaymentMethod;
		}
		public Guid DebtId { get; set; }
		public DateTime PaymentDate { get; set; }
		public decimal AmountPaid { get; set; }
		public PaymentMethodType TypeOfPayment { get; set; }

	}

}
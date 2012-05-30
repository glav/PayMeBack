using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public class DebtPaymentPlan : BaseModel
	{
		public DebtPaymentPlan() {}

		public DebtPaymentPlan(Data.DebtPaymentPlan debtPaymentPlanDetail)
		{
			PaymentPeriod = (Domain.PaymentPeriod)debtPaymentPlanDetail.PaymentPeriod;
			StartDate = debtPaymentPlanDetail.StartDate;
			ExpectedEndDate = debtPaymentPlanDetail.ExpectedEndDate;
			InitialPayment = debtPaymentPlanDetail.InitialPayment.HasValue ? debtPaymentPlanDetail.InitialPayment.Value :0;
			IsOutstanding = debtPaymentPlanDetail.IsOutstanding.HasValue ? debtPaymentPlanDetail.IsOutstanding.Value : true;
			if (debtPaymentPlanDetail.UserDetail !=null)
			{
				UserWhoOwesDebt = new User(debtPaymentPlanDetail.UserDetail);
			}
			if (debtPaymentPlanDetail.Debt != null)
			{
				DebtOwed = new Debt(debtPaymentPlanDetail.Debt);
			}
		}

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
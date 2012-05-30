using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public class Debt : BaseModel
	{
		public Debt() {}

		public Debt(Data.Debt debtDetail)
		{
			TotalAmountOwed = debtDetail.TotalAmountOwed;
			ReasonForDebt = debtDetail.ReasonForDebt;
			Notes = debtDetail.Notes;
		}
		public decimal TotalAmountOwed { get; set; }
		public string ReasonForDebt { get; set; }
		public string Notes { get; set; }

	}
}
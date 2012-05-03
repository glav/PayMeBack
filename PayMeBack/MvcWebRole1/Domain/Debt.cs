using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public class Debt : BaseModel
	{
		public decimal TotalAmountOwed { get; set; }
		public string ReasonForDebt { get; set; }
		public string Notes { get; set; }

	}
}
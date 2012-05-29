using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public class UserPaymentPlan: BaseModel
	{
		public User User { get; set; }
		public List<DebtPaymentPlan> DebtsOwedToOthers { get; set; }
		public List<DebtPaymentPlan> DebtsOwedToMe { get; set; } 
	}
}
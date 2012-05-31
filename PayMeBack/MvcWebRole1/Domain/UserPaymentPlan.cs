using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public class UserPaymentPlan: BaseModel
	{
		public User User { get; set; }
		public List<Debt> DebtsOwedToOthers { get; set; }
		public List<Debt> DebtsOwedToMe { get; set; } 
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Core.Domain
{
	public class UserPaymentPlan: BaseModel
	{
		public User User { get; set; }
		public List<Debt> DebtsOwedToOthers { get; set; }
		public List<Debt> DebtsOwedToMe { get; set; }
		public DateTime DateCreated {get; set;}
	}
}
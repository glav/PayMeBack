using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public class User : BaseModel
	{
		public string EmailAddress { get; set; }
		public string FirstNames { get; set; }
		public string Surname { get; set; }
		public string Password { get; set; }
		public List<DebtPaymentPlan> DebtsOwedToOthers { get; set; }
		public List<DebtPaymentPlan> DebtsOwedToMe { get; set; } 
	}
}
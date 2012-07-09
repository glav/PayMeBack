using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain
{
	public class UserPaymentPlan: BaseModel
	{
		public UserPaymentPlan() { }

		public UserPaymentPlan(UserPaymentPlanDetail detail)
		{
			if (detail.UserDetail != null)
			{
				User = new User(detail.UserDetail);
			}
			else
			{
				User = new User {Id = detail.UserId};
			}
			DateCreated = detail.DateCreated;
		}
		public User User { get; set; }
		public List<Debt> DebtsOwedToOthers { get; set; }
		public List<Debt> DebtsOwedToMe { get; set; }
		public DateTime DateCreated {get; set;}
	}
}
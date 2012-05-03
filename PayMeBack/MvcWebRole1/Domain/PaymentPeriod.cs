using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public enum PaymentPeriod
	{
		AdHoc,  // pay amounts in no regular interval, just whenever they can
		Daily,
		Weekly,
		Fortnightly,
		Monthly,
		BiMonthly,
		Quarterly,
		HalfYearly,
		Yearly
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Client.Proxies
{
	public class DebtProxy : BaseProxy
	{
		public DebtProxy()
			: base()
		{
			PagingEnabled = false;
		}

		public DebtProxy(string bearerToken)
			: base(bearerToken)
		{
			PagingEnabled = false;
		}

		public override string RequestPrefix
		{
			get { return ResourceNames.Debt; }
		}

		public void GetDebtPaymentPlan(Guid userId)
		{
			
		}
	}
}

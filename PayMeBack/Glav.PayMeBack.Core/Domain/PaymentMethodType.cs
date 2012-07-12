using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Core.Domain
{
	public enum PaymentMethodType
	{
		Unknown=0,
		Cash=1,
		BankTransfer=2,
		Services=3,
		Goods=4
	}
}
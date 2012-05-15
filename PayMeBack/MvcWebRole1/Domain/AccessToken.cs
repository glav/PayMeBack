using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public class AccessToken
	{
		public Guid UserId { get; set; }
		public Guid Token { get; set; }
		public DateTime TokenExpiry { get; set; }
	}
}
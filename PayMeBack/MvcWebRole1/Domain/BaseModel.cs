using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain
{
	public abstract class BaseModel
	{
		public Guid Id { get; set; }
	}
}
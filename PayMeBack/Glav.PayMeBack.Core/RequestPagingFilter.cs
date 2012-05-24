using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.PayMeBack.Core
{
	public class RequestPagingFilter
	{
		public RequestPagingFilter()
		{
			Page = 1;
			PageSize = 25;
		}
		public int Page { get; set; }
		public int PageSize { get; set; }
	}
}

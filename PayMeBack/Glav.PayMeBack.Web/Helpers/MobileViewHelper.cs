using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web
{
	public static class MobileViewHelper
	{
		public const string MobileContextKey = "IsMobile";

		public static bool IsCurrentRequestMobileView()
		{
			if (System.Web.HttpContext.Current == null)
			{
				return false;
			}

			var context = System.Web.HttpContext.Current;
			if (context.Items.Contains(MobileContextKey))
			{
				var isMobile = (bool)context.Items[MobileContextKey];
				return isMobile;
			}

			return false;
		}

		public static void InterrogateRequestForMobileCapability()
		{
			if (System.Web.HttpContext.Current == null)
			{
				return;
			}

			var context = System.Web.HttpContext.Current;
			var userAgent = context.Request.UserAgent.ToLowerInvariant();
			if (userAgent.Contains("mobi") || userAgent.Contains("iphone"))
			{
				context.Items[MobileContextKey] = true;
			}
			else
			{
				context.Items[MobileContextKey] = false;
			}
		}
	}
}
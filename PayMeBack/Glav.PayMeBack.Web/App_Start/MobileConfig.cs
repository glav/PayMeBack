using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace Glav.PayMeBack.Web.App_Start
{
    public class MobileConfig
    {
        public static void Register()
        {
			DisplayModeProvider.Instance.Modes.Insert(0, new
			DefaultDisplayMode("Mobile")
			{
				ContextCondition = (context =>
				{
					var userAgent = context.GetOverriddenUserAgent();
					var useMobileView = (userAgent.IndexOf("Mobi", StringComparison.OrdinalIgnoreCase) >= 0) || (userAgent.IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) >= 0);

					// This is for debugging with Firefox - comment out for real use
					//if (!useMobileView)
					//{
					//    useMobileView = userAgent.IndexOf("Mozilla", StringComparison.OrdinalIgnoreCase) >= 0;
					//}

					context.Items[MobileViewHelper.MobileContextKey] = useMobileView;
					return useMobileView;
				})
			});

        }
    }
}
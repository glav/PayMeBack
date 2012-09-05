using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Domain.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Glav.PayMeBack.Web.Helpers
{
    public static class HtmlCurrencyHelper
    {
        /// <summary>
        /// Helper to format an amount based on the current users culture
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string ToCurrencyDisplay(this HtmlHelper helper, decimal amount)
        {
            var cultureService = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ICultureFormattingEngine)) as ICultureFormattingEngine;
            var webMembershipManager = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IWebMembershipManager)) as IWebMembershipManager;
            if (cultureService != null && webMembershipManager != null)
            {
                var user = webMembershipManager.GetUserFromRequestCookie();
                return cultureService.ConvertAmountToCurrencyForDisplay(user, amount);
            }
            return amount.ToString();
        }

        public static string ToDisplayDate(this HtmlHelper helper, DateTime? date)
        {
            var cultureService = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ICultureFormattingEngine)) as ICultureFormattingEngine;
            var webMembershipManager = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IWebMembershipManager)) as IWebMembershipManager;
            if (cultureService != null && webMembershipManager != null)
            {
                var user = webMembershipManager.GetUserFromRequestCookie();
                var displayDate = cultureService.ConvertDateToUserPreferenceFormatForDisplay(user, date);
                if (string.IsNullOrWhiteSpace(displayDate))
                {
                    displayDate = "Never";
                }
                return displayDate;
            }
            return date.HasValue ? date.Value.Date.ToShortDateString() : "Never";
        }
        public static string ToDisplayTime(this HtmlHelper helper, DateTime? time)
        {
            var cultureService = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ICultureFormattingEngine)) as ICultureFormattingEngine;
            var webMembershipManager = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IWebMembershipManager)) as IWebMembershipManager;
            if (cultureService != null && webMembershipManager != null)
            {
                var user = webMembershipManager.GetUserFromRequestCookie();
                return cultureService.ConvertDateToUserPreferenceFormatForDisplay(user, time);
            }
            return time.HasValue ? time.Value.Date.ToShortTimeString() : string.Empty;
        }
    }
}
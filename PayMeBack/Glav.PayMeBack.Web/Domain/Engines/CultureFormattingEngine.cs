using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Engines
{
    public class CultureFormattingEngine : ICultureFormattingEngine
    {
        public string ConvertAmountToCurrencyForDisplay(Core.Domain.User user, decimal amount)
        {
            //TODO: Work out users culture - probably get via a setting or use thread default
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var displayAmount = string.Format(culture.NumberFormat,"{0:C}", amount);

            return displayAmount;
        }
        public string ConvertDateToUserPreferenceFormatForDisplay(Core.Domain.User user, DateTime? date)
        {
            if (date == null)
            {
                return string.Empty;
            }
            //TODO: Work out users culture - probably get via a setting or use thread default
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var displayDate = date.Value.Date.ToString("dd MMM yyyy", culture.DateTimeFormat);

            return displayDate;
        }
        public string ConvertTimeToUserPreferenceFormatForDisplay(Core.Domain.User user, DateTime? time)
        {
            if (time == null)
            {
                return string.Empty;
            }
            //TODO: Work out users culture - probably get via a setting or use thread default
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var displayTime = time.Value.TimeOfDay.ToString("HH:mm", culture.DateTimeFormat);

            return displayTime;
        }


        public DateTime? ConvertTextFromCultureFormatToDateTime(Core.Domain.User user, string dateText)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            DateTime parsed;
            if (DateTime.TryParse(dateText, culture.DateTimeFormat, System.Globalization.DateTimeStyles.AdjustToUniversal, out parsed))
            {
                return parsed;
            }
            return null;
        }
    }
}
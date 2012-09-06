using Glav.PayMeBack.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Engines
{
    public interface ICultureFormattingEngine
    {
        string ConvertAmountToCurrencyForDisplay(User user, decimal amount);
        string ConvertDateToUserPreferenceFormatForDisplay(Core.Domain.User user, DateTime? date);
        string ConvertTimeToUserPreferenceFormatForDisplay(Core.Domain.User user, DateTime? time);
    }
}
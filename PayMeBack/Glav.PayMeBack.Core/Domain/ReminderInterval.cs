using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.PayMeBack.Core.Domain
{
    public class ReminderInterval
    {
        public IntervalFrequency Frequency { get; set; }
        public int FrequencyCount { get; set; }   // eg. every 3 days or every 2 weeks
        public DayOfWeek WeekDay { get; set; }  // If set to weekly
    }

    public enum IntervalFrequency
    {
        Unspecified = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Yearly = 4
    }
}

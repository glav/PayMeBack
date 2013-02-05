using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Services
{
    public class NotificationService : INotificationService
    {
        public Core.Domain.NotificationOptions GetNotificationOptionsForUserDebt(Guid userId, Guid debtId)
        {
            // return dummy for now
            return new Core.Domain.NotificationOptions
            {
                UserId = userId,
                DebtId=debtId,
                NotificationMethod = Core.Domain.NotificationType.Email,
                NotificationEmailAddress = "dummy@stub.com",
                Interval = new Core.Domain.ReminderInterval { Frequency = Core.Domain.IntervalFrequency.Weekly, FrequencyCount = 1, WeekDay = DayOfWeek.Friday },
            };
        }

        public Data.DataAccessResult UpdateNotificationOptionsForUserDebt(Core.Domain.NotificationOptions notifyOptions)
        {
            // return dummy for now
            return new Data.DataAccessResult { WasSuccessfull = true };
        }
    }
}
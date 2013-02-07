using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.PayMeBack.Core.Domain
{
    public class NotificationOptions : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid DebtId { get; set; }
        public NotificationType NotificationMethod { get; set; }
        public string NotificationEmailAddress { get; set; }
        public string NotificationSmsNumber { get; set; }
        public ReminderInterval Interval { get; set; }
    }


}

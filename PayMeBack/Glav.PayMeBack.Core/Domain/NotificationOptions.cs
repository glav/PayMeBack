using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Glav.PayMeBack.Core.Domain
{
    public class NotificationOptions : BaseModel
    {
        public NotificationOptions()
        {
            Interval = new ReminderInterval();
        }
        public Guid UserId { get; set; }
        public Guid DebtId { get; set; }

        [Display(Description="Notification Method", ShortName="Method")]
        public NotificationType NotificationMethod { get; set; }
        
        [Display(Description="Email Address")]
        [DataType(DataType.EmailAddress)]
        public string NotificationEmailAddress { get; set; }

        [Display(Description="Sms Number")]
        public string NotificationSmsNumber { get; set; }

        public ReminderInterval Interval { get; set; }
    }


}

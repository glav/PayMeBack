//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Glav.PayMeBack.Web.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class NotificationOptionsDetail
    {
        public System.Guid Id { get; set; }
        public System.Guid UserId { get; set; }
        public System.Guid DebtId { get; set; }
        public int Method { get; set; }
        public string EmailAddress { get; set; }
        public string SmsPhoneNumber { get; set; }
        public int ReminderIntervalFrequency { get; set; }
        public Nullable<int> ReminderIntervalCount { get; set; }
        public Nullable<int> ReminderIntervalDayOfWeek { get; set; }
    }
}

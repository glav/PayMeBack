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
    
    public partial class UserDetail
    {
        public UserDetail()
        {
            this.DebtDetails = new HashSet<DebtDetail>();
            this.UserPaymentPlanDetails = new HashSet<UserPaymentPlanDetail>();
        }
    
        public System.Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstNames { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public bool IsValidated { get; set; }
    
        public virtual ICollection<DebtDetail> DebtDetails { get; set; }
        public virtual ICollection<UserPaymentPlanDetail> UserPaymentPlanDetails { get; set; }
    }
}

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
    
    public partial class DebtPaymentInstallmentDetail
    {
        public System.Guid Id { get; set; }
        public System.Guid DebtId { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public int PaymentMethod { get; set; }
    
        public virtual DebtDetail DebtDetail { get; set; }
    }
}

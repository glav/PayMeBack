﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PayMeBackEntities : DbContext
    {
        public PayMeBackEntities()
            : base("name=PayMeBackEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<OAuthToken> OAuthTokens { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<UserPaymentPlan> UserPaymentPlans { get; set; }
        public DbSet<DebtPaymentInstallment> DebtPaymentInstallments { get; set; }
        public DbSet<UsageLog> UsageLogs { get; set; }
    }
}

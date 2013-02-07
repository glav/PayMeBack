using Glav.PayMeBack.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Engines
{
    public class AuthorisationEngine :IAuthorisationEngine
    {
        public bool EnsureUserCanModifyDebt(Guid debtId, UserPaymentPlan usersPlan)
        {
            //TODO: Examine scope associated with the user and return false if readonly
            //TODO: Optionally check permissions with the app

            // If the debt is a part of the debts that the user manages (ie. those debts
            // that are owed to the user), then they can delete it
            var userCanRemoveDebt = (usersPlan.DebtsOwedToMe.Count(d => d.Id == debtId) > 0);

            return userCanRemoveDebt;
        }
    }
}
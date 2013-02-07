using Glav.PayMeBack.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.PayMeBack.Web.Domain.Engines
{
    public interface IAuthorisationEngine
    {
        bool EnsureUserCanModifyDebt(Guid debtId, UserPaymentPlan usersPlan);
    }
}

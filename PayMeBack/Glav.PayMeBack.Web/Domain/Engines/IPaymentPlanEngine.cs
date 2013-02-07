using Glav.PayMeBack.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.PayMeBack.Web.Domain.Engines
{
    public interface IPaymentPlanEngine
    {
        UserPaymentPlan GetPaymentPlan(Guid userId);
        void InvalidateCacheForAllUsersRelatedToPlan(UserPaymentPlan usersPaymentPlan);
        string GetCacheKeyForUserPaymentPlan(Guid userId);
    }
}

using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.PayMeBack.Web.Domain.Services
{
    public interface INotificationService
    {
        NotificationOptions GetNotificationOptionsForUserDebt(Guid userId, Guid DebtId);
        DataAccessResult UpdateNotificationOptionsForUserDebt(NotificationOptions notifyOptions);
    }
}

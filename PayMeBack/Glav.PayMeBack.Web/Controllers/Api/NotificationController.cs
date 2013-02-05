using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Glav.PayMeBack.Web.Controllers.Api
{
    public class NotificationController : ApiController
    {
        private INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public ApiResponse PostUpdateNotificationOptions(NotificationOptions notificationOptions)
        {
            var response = new ApiResponse();
            var result = _notificationService.UpdateNotificationOptionsForUserDebt(notificationOptions);
            response.IsSuccessful = result.WasSuccessfull;
            if (!result.WasSuccessfull)
            {
                response.ErrorMessages.AddRange(result.Errors);
            }
            return response;
        }

        public NotificationOptions GetOptions([FromUri] User user, Guid debtId)
        {
            var response = _notificationService.GetNotificationOptionsForUserDebt(user.Id, debtId);
            return response;
        }
    }
}

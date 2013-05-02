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
    public class PaymentController : ApiController
    {
        private IPaymentPlanService _paymentPlanService;

        public PaymentController(IPaymentPlanService paymentPlanService)
        {
            _paymentPlanService = paymentPlanService;
        }

        public ApiResponse Put([FromUri] User user, DebtPaymentInstallment payment)
        {
            var response = new ApiResponse { IsSuccessful = false };

            var addResult = _paymentPlanService.AddPaymentInstallmentToPlan(user.Id, payment);
            if (addResult.WasSuccessfull)
            {
                response.IsSuccessful = true;
                return response;
            }

            response.ErrorMessages = addResult.Errors;
            return response;

        }
    }
}

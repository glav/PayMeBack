using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.PayMeBack.Web.Models;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Controllers.Api
{
    public class DebtsController : ApiController
    {
    	private IPaymentPlanService _paymentPlanService;

    	public DebtsController(IPaymentPlanService paymentPlanService)
    	{
    		_paymentPlanService = paymentPlanService;
    	}

    	// GET /api/people
		public UserPaymentPlan Get([FromUri] User user)
		{
			return _paymentPlanService.GetPaymentPlan(user.Id);
        }

		public ApiResponse Post(UserPaymentPlan paymentPlan)
		{
			var result = _paymentPlanService.UpdatePaymentPlan(paymentPlan);
			return result.ToApiResponse();
		}

    }
}

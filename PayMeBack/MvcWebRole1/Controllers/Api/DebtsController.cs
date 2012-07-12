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
		private IUserEngine _userEngine;

    	public DebtsController(IPaymentPlanService paymentPlanService, IUserEngine userEngine)
    	{
    		_paymentPlanService = paymentPlanService;
			_userEngine = userEngine;
    	}

    	// GET /api/people
		public UserPaymentPlan Get(Guid userId)
		{
			return _paymentPlanService.GetPaymentPlan(userId);
        }

		public ApiResponse Post(UserPaymentPlan paymentPlan)
		{
			var result = _paymentPlanService.UpdatePaymentPlan(paymentPlan);
			var response = new ApiResponse
			               	{
			               		IsSuccessful = result.WasSuccessfull,
			               		ErrorMessage = result.ToSingleMessage()
			               	};
			return response;
		}
    }
}

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

		/// <summary>
		/// Returns an existing payment plan, or a new one if one does not
		/// exist for the user. The user is inferred from the user making the
		/// request
		/// </summary>
		/// <param name="user">The user to retrieve the paymentplan for. This is
		/// retrieving from the users access token who is making the request</param>
		/// <returns>A <see cref="UserPaymentPlan"/></returns>
		public UserPaymentPlan Get([FromUri] User user)
		{
			return _paymentPlanService.GetPaymentPlan(user.Id);
        }

		/// <summary>
		/// Updates an existing payment plan for the user making the request
		/// </summary>
		/// <param name="paymentPlan">The payment plan to be updated.</param>
		/// <returns>A standard ApiResponse containing a success indicator or a
		/// series of errors if unsuccessfull</returns>
		public ApiResponse Post(UserPaymentPlan paymentPlan)
		{
			var result = _paymentPlanService.UpdatePaymentPlan(paymentPlan);
			return result.ToApiResponse();
		}

		/// <summary>
		/// Removes/deletes a debt that is owed to or managed bythe user
		/// </summary>
		/// <param name="user"></param>
		/// <param name="debtId"></param>
		/// <returns></returns>
		public ApiResponse Delete([FromUri] User user, Guid debtId)
		{
			var result = _paymentPlanService.RemoveDebt(user.Id,debtId);
			return result.ToApiResponse();
		}

    }
}

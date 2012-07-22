using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Domain.Services;

namespace Glav.PayMeBack.Web.Controllers.Api
{
    public class SummaryController : ApiController
    {
		private IPaymentPlanService _paymentPlanService;

		public SummaryController(IPaymentPlanService paymentPlanService)
		{
			_paymentPlanService = paymentPlanService;
		}

		/// <summary>
		/// Returns a Summarised version of a users debts owed and owing.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public DebtSummary Get([FromUri] User user)
		{
			return _paymentPlanService.GetDebtSummaryForUser(user.Id);
		}

	}
}

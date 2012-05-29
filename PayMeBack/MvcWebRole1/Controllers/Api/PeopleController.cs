using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Domain.Engines;

namespace Glav.PayMeBack.Web.Controllers.Api
{
    public class PeopleController : ApiController
    {
    	private IPaymentPlanService _paymentPlanService;
		private IUserEngine _userEngine;

    	public PeopleController(IPaymentPlanService paymentPlanService, IUserEngine userEngine)
    	{
    		_paymentPlanService = paymentPlanService;
			_userEngine = userEngine;
    	}

    	// GET /api/people
		public IEnumerable<DebtPaymentPlan> Get(Guid userId, Guid accessTokenId)
        {
        	return OweMeMoney(userId, accessTokenId);
        }

		public IEnumerable<DebtPaymentPlan> OweMeMoney(Guid userId, Guid accessTokenId)
		{

			var list = new List<DebtPaymentPlan>();
			return list;
		}

		public IEnumerable<DebtPaymentPlan> OweMoneyTo(Guid userId, Guid accessTokenId)
		{
			var list = new List<DebtPaymentPlan>();
			return list;
		}

    }
}

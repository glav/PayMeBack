using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Controllers
{
	[Authorize]
    public class SummaryController : Controller
    {
    	private IPaymentPlanService _paymentPlanService;
    	private IWebMembershipManager _webMembershipManager;

		public SummaryController(IWebMembershipManager webMembershipManager, IPaymentPlanService paymentPlanService)
		{
			_webMembershipManager = webMembershipManager;
			_paymentPlanService = paymentPlanService;
		}
		[Authorize]
		public ActionResult Index()
        {
        	var user = _webMembershipManager.GetUserFromRequestCookie();
        	var model = _paymentPlanService.GetPaymentPlan(user.Id);
            return View(model);
        }

    }
}

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
            try
            {
                var user = _webMembershipManager.GetUserFromRequestCookie();
                var model = _paymentPlanService.GetDebtSummaryForUser(user.Id);
                return View(model);
            }
            catch (Exception ex)
            {
                //TODO: Log it
            }
            return View();
        }

        [Authorize]
        public PartialViewResult DebtsOwedToMe()
        {
            try
            {
                var user = _webMembershipManager.GetUserFromRequestCookie();
                var model = _paymentPlanService.GetDebtSummaryForUser(user.Id);
                return PartialView("_DebtSummaryList", model);
            }
            catch (Exception ex)
            {
                //TODO: log it
            }
            return PartialView("_DebtSummaryList");
        }

    }
}

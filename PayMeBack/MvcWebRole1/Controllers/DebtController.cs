﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Web.Controllers
{
    public class DebtController : Controller
    {
        private IPaymentPlanService _paymentPlanService;
        private IWebMembershipManager _webMembershipManager;

        public DebtController(IPaymentPlanService paymentPlanService, IWebMembershipManager webMembershipManager)
        {
            _paymentPlanService = paymentPlanService;
            _webMembershipManager = webMembershipManager;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(string emailAddress, string amountOwed, string debtReason, string initialAmountPaid, string notes, string paymentPeriod, string expectedEndDate )
        {
            var user = _webMembershipManager.GetUserFromRequestCookie();
            var debt = new Debt {ReasonForDebt = debtReason, Notes = notes};
            debt.UserWhoOwesDebt = new User {EmailAddress = emailAddress};
            decimal amt;
            if (decimal.TryParse(amountOwed, out amt))
            {
                debt.TotalAmountOwed = amt;
            }
            if (decimal.TryParse(initialAmountPaid, out amt))
            {
                debt.InitialPayment = amt;
            }
            var addResult = _paymentPlanService.AddDebtOwed(user.Id, debt);
            if (addResult.WasSuccessfull)
            {
                return Json(new { success = true });    
            }
            return Json(new { success = false,errorMessage = addResult.Errors.Count > 0 ? addResult.Errors[0] : string.Empty });
        }

    }
}

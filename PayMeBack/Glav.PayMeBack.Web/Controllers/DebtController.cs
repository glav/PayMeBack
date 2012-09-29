using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.PayMeBack.Web.Resources;

namespace Glav.PayMeBack.Web.Controllers
{
    public class DebtController : Controller
    {
        private IPaymentPlanService _paymentPlanService;
        private IWebMembershipManager _webMembershipManager;
        private ICultureFormattingEngine _cultureEngine;

        public DebtController(IPaymentPlanService paymentPlanService, IWebMembershipManager webMembershipManager, ICultureFormattingEngine cultureEngine)
        {
            _paymentPlanService = paymentPlanService;
            _webMembershipManager = webMembershipManager;
            _cultureEngine = cultureEngine;
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

        [HttpPost]
        public JsonResult AddPayment(string debtId, decimal? amount, string date, int? paymentType )
        {
            var user = _webMembershipManager.GetUserFromRequestCookie();
            if (string.IsNullOrWhiteSpace(debtId) || !amount.HasValue || string.IsNullOrWhiteSpace(date) || !paymentType.HasValue)
            {
                return Json(new { success = false, errorMessage = ErrorMessages.AddPaymentInstallment_BadData });
            }

            Guid id;
            if (!Guid.TryParse(debtId, out id))
            {
                return Json(new { success = false, errorMessage = ErrorMessages.BadDebtId });
            }
            var debtPayment = new DebtPaymentInstallment { DebtId = id };
            debtPayment.AmountPaid = amount.Value;
            var paymentDate = _cultureEngine.ConvertTextFromCultureFormatToDateTime(user,date);
            if ( paymentType == null)
            {
                return Json(new { success = false, errorMessage = ErrorMessages.BadDateFormat });
            }

            debtPayment.PaymentDate = paymentDate.Value;
            debtPayment.TypeOfPayment = PaymentMethodType.Unknown;

            try
            {
                if (paymentType.HasValue)
                {
                    debtPayment.TypeOfPayment = (PaymentMethodType)paymentType.Value;
                }
            }
            catch
            {
                return Json(new { success = false, errorMessage = ErrorMessages.PaymentTypeNotSupported });
            }

            var addResult = _paymentPlanService.AddPaymentInstallmentToPlan(user.Id, debtPayment);
            if (addResult.WasSuccessfull)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, errorMessage = addResult.Errors.Count > 0 ? addResult.Errors[0] : string.Empty });
        }

        [HttpDelete]
        public JsonResult Delete(string debtId)
        {
            if (!string.IsNullOrWhiteSpace(debtId))
            {
                try
                {
                    var user = _webMembershipManager.GetUserFromRequestCookie();
                    var id = new Guid(debtId);
                    var result = _paymentPlanService.RemoveDebt(user.Id,id);
                    string total = null;
                    if (result.WasSuccessfull)
                    {
                        var summary = _paymentPlanService.GetDebtSummaryForUser(user.Id);
                        total = _cultureEngine.ConvertAmountToCurrencyForDisplay(user, summary.TotalAmountOwedToYou);
                    }
                    return Json(new { success = result.WasSuccessfull, errorMessage = result.Errors.Count > 0 ? result.Errors[0] : string.Empty, total = total });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = "There was error processing your delete request. Please retry" });
                }
            }
            return Json(new { success = true, errorMessage = string.Empty });
        }

    }
}

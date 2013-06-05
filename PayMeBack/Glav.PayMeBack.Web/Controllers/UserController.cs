using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.PayMeBack.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Glav.PayMeBack.Web.Controllers
{
    public class UserController : Controller
    {
        private IWebMembershipManager _webMembershipManager;
        private IUserEngine _userEngine;

        public UserController(IWebMembershipManager webMembershipManager, IUserEngine userEngine)
        {
            _webMembershipManager = webMembershipManager;
            _userEngine = userEngine;
        }

        [Authorize]
        [HttpGet]
        public JsonResult Index()
        {
            var viewModel = new AccountSettingsViewModel();
            viewModel.UserDetails = _webMembershipManager.GetUserFromRequestCookie();
            return Json(viewModel,JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        [HttpPost]
        public JsonResult Update(AccountSettingsViewModel model)
        {
            try
            {
                // todo: call update method
                _userEngine.SaveOrUpdateUser(model.UserDetails, model.NewPassword);
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false
                });
            }

        }
    }
}

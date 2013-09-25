using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Domain.Services;
using System.Web.Security;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Web.Controllers
{
    public class MembershipController : Controller
    {
        private IWebMembershipManager _membershipManager;

        public MembershipController(IWebMembershipManager membershipManager)
        {
            _membershipManager = membershipManager;
        }
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Signup(string email, string password, string firstname, string surname)
        {
            var result =  _membershipManager.SignupAndIssueCookie(email, password,firstname,surname);
            return FormJsonResult(result);
        }

        public ActionResult Logout()
        {
            _membershipManager.Logout();
            return Redirect("~");
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Login(string email, string password)
        {
            var result = _membershipManager.LoginAndIssueCookie(email, password);
            return FormJsonResult(result); 
        }

        private JsonResult FormJsonResult(MembershipResponseModel responseModel)
        {
            return Json(new
            {
                success = responseModel.IsSuccessfull,
                firstname = responseModel.Firstname,
                surname = responseModel.Surname,
                email = responseModel.Email,
                error=responseModel.IsSuccessfull ?string.Empty : responseModel.Error
            });
        }
    }
}

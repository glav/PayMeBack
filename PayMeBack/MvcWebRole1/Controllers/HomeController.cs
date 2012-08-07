using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glav.PayMeBack.Web.Models;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Controllers
{
	public class HomeController : Controller
	{
        private IWebMembershipManager _webMembershipManager;

        public HomeController(IWebMembershipManager webMembershipManager)
        {
            _webMembershipManager = webMembershipManager;
        }
		public ActionResult Index()
		{
			return View(CreateModel());
		}

		private HomeActionModel CreateModel()
		{
			var model = new HomeActionModel
			            	{
								InformationalMessage = "Info Message Placeholder"
			            	};
            var user = _webMembershipManager.GetUserFromRequestCookie();
            if (user != null)
            {
                model.UserName = user.FirstNames;
            }
                return model;
		}
	}
}

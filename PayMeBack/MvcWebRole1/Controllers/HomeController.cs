﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glav.PayMeBack.Web.Models;

namespace Glav.PayMeBack.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View(CreateModel());
		}

		private HomeActionModel CreateModel()
		{
			var model = new HomeActionModel
			            	{
								IsAuthenticated = System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated
			            	};
			model.InformationalMessage = "info message";
			return model;
		}
	}
}

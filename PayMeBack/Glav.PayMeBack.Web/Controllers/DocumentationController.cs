using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glav.PayMeBack.Web.Domain.Engines;

namespace Glav.PayMeBack.Web.Controllers
{
    public class DocumentationController : Controller
    {
		private IHelpEngine _helpEngine;

		[Obsolete("This CTOR is here only until I hook up the Autofac MVC integration for ServiceResolver")]
    	public DocumentationController()
    	{
    		_helpEngine = new HelpEngine();
    	}

		public DocumentationController(IHelpEngine helpEngine)
		{
			_helpEngine = helpEngine;
		}
		public ActionResult Index()
        {
        	return Api();
        }

		public ActionResult Api()
		{
			return View("Api",_helpEngine.GetApiHelpInformation());
		}

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Web.Domain.Engines;

namespace Glav.PayMeBack.Web.Controllers.Api
{
    public class HelpController : ApiController
    {
    	private IHelpEngine _helpEngine;

    	public HelpController(IHelpEngine helpEngine)
    	{
    		_helpEngine = helpEngine;
    	}
		
		public ApiHelp GetHelp()
		{
			return _helpEngine.GetApiHelpInformation();
		}
    }
}

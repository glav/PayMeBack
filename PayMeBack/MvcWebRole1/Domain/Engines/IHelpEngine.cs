using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Web.Domain.Engines
{
	public interface IHelpEngine
	{
		ApiHelp GetApiHelpInformation();
	}
}
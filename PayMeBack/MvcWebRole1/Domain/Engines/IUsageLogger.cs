using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;

namespace Glav.PayMeBack.Web.Domain.Engines
{
	public interface IUsageLogger
	{
		void LogRequestInformation(HttpRequestMessage request);
		void LogRequestInformation(HttpResponseMessage response);
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Glav.PayMeBack.Web.Domain.Engines;

namespace Glav.PayMeBack.Web.Framework
{
	public class ApiUsageHandler : DelegatingHandler
	{
		private IUsageLogger _usageLogger;

		public ApiUsageHandler(IUsageLogger usageLogger)
		{
			_usageLogger = usageLogger;
		}
		protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			_usageLogger.LogRequestInformation(request);

			var responseTask = base.SendAsync(request, cancellationToken);

			responseTask.ContinueWith(task =>
				{
					//_usageLogger.LogResponseInformation(task.Result);
				});

			return responseTask;
		}
	}
}
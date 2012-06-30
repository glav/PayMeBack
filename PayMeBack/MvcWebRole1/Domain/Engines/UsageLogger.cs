using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.IO;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain.Engines
{
	public class ApiUsageLoggerEngine : IUsageLogger
	{
		ICrudRepository _crudRepository;

		public ApiUsageLoggerEngine(ICrudRepository crudRepository)
		{
			_crudRepository = crudRepository;
		}

		public void LogRequestInformation(HttpRequestMessage request)
		{

		}

		public void LogRequestInformation(HttpResponseMessage response)
		{

		}


		private void ExtractContentFromRequestAndLog(HttpRequestMessage request)
		{
			request.Content.LoadIntoBufferAsync().ContinueWith(t =>
				{
					var memStream = new MemoryStream();
					request.Content.CopyToAsync(memStream).ContinueWith(task =>
						{
							memStream.Position = 0;
							using (var rdr = new StreamReader(memStream))
							{
								var content = rdr.ReadToEnd();
								// Do something with the contents...
							}
						});
				});
		}
	}

}
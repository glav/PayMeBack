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
			var logRecord = new UsageLog();
			logRecord.HttpMethod = request.Method.ToString();
			logRecord.IsRequest = true;
			logRecord.TimeOfOperation = DateTime.UtcNow;
			logRecord.Uri = request.RequestUri.AbsoluteUri;

			//if (request.Content != null)
			//{
			//    request.Content.LoadIntoBufferAsync().ContinueWith(t =>
			//    {
			//        var memStream = new MemoryStream();
			//        request.Content.CopyToAsync(memStream).ContinueWith(task =>
			//        {
			//            memStream.Position = 0;
			//            using (var rdr = new StreamReader(memStream))
			//            {
			//                logRecord.Body = rdr.ReadToEnd();
			//                _crudRepository.Insert<UsageLog>(logRecord);
			//            }
			//        });
			//    });
			//}
			//else
			//{
				_crudRepository.Insert<UsageLog>(logRecord);
			//}

		}


		public void LogResponseInformation(HttpResponseMessage response)
		{
			var logRecord = new UsageLog();
			logRecord.HttpMethod = response.RequestMessage.Method.ToString();
			logRecord.IsRequest = false;
			logRecord.TimeOfOperation = DateTime.UtcNow;
			logRecord.Uri = response.RequestMessage.RequestUri.AbsoluteUri;
			logRecord.StatusCode = (int)response.StatusCode;

			//if (response.Content != null)
			//{
			//    response.Content.LoadIntoBufferAsync().ContinueWith(t =>
			//    {
			//        var memStream = new MemoryStream();
			//        response.Content.CopyToAsync(memStream).ContinueWith(task =>
			//        {
			//            memStream.Position = 0;
			//            using (var rdr = new StreamReader(memStream))
			//            {
			//                logRecord.Body = rdr.ReadToEnd();
			//                _crudRepository.Insert<UsageLog>(logRecord);
			//            }
			//        });
			//    });
			//}
			//else
			//{
				_crudRepository.Insert<UsageLog>(logRecord);
			//}
		}
	}

}
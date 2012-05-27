﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Glav.PayMeBack.Client
{
	public class ProxyResponse
	{
		public ProxyResponse(string rawResponse, bool wasSuccesfull, HttpStatusCode statusCode, string reasonCode)
		{
			RawResponse = rawResponse;
			IsRequestSuccessfull = wasSuccesfull;
			StatusCode = statusCode;
			ReasonCode = reasonCode;

		}
		public string RawResponse { get; private set; }
		public bool IsRequestSuccessfull { get; private set; }
		public HttpStatusCode StatusCode { get; private set; }
		public string ReasonCode { get; private set; }
	}

	public class ProxyResponse<T> : ProxyResponse
	{
		public ProxyResponse(string rawResponse, T dataObject, bool wasSuccesfull, HttpStatusCode statusCode, string reasonCode)
			: base(rawResponse, wasSuccesfull,statusCode,reasonCode)
		{
			DataObject = dataObject;

		}
		public T DataObject { get; private set; }
	}
}

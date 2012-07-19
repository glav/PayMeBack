using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Glav.PayMeBack.Core.Domain;

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
			ErrorMessages = new List<string>();

		}

		public string RawResponse { get; private set; }
		public bool IsRequestSuccessfull { get; private set; }
		public HttpStatusCode StatusCode { get; private set; }
		public string ReasonCode { get; private set; }
		public List<string> ErrorMessages { get; set; } 

		public void MapApiResult(ApiResponse apiResponse)
		{
			if (apiResponse != null)
			{
				IsRequestSuccessfull = apiResponse.IsSuccessful;
				if (apiResponse.ErrorMessages != null && apiResponse.ErrorMessages.Count > 0)
				{
					ErrorMessages.AddRange(apiResponse.ErrorMessages);
				}
			}
		}
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

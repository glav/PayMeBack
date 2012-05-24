using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.PayMeBack.Client
{
	public enum RequestContentType
	{
		ApplicationJson,
		ApplicationXml
	}

	public static class RequestContentTypeExtensions
	{
		public static string AsContentTypeString(this RequestContentType requestContentType)
		{
			var contentType = "application/json";
			if (requestContentType == RequestContentType.ApplicationXml)
			{
				contentType = "application/xml";
			}
			return contentType;
		}
	}
}

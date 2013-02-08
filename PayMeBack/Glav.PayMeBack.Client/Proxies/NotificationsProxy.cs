using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Client.Proxies
{
	public class NotificationsProxy : BaseProxy
	{
		public NotificationsProxy()
			: base()
		{
			PagingEnabled = false;
		}

        public NotificationsProxy(string bearerToken)
			: base(bearerToken)
		{
			PagingEnabled = false;
		}

		public override string RequestPrefix
		{
			get { return ResourceNames.Notification; }
		}

		/// <summary>
		/// Service uses the current access token to retrieve the user so we dont need to
		/// pass it anything else like a user id
		/// </summary>
		/// <returns></returns>
		public ProxyResponse<NotificationOptions> GetNotificationOptions()
		{
            ProxyResponse<NotificationOptions> proxyResult = null;

			ContentType = RequestContentType.ApplicationJson;
			OperationMethod = HttpMethod.Get;
			var uri = base.GetRequestUri(null);
			var response = GetResponse<NotificationOptions>(uri);
			if (response.IsRequestSuccessfull)
			{
				proxyResult = new ProxyResponse<NotificationOptions>(response.RawResponse, response.DataObject, true, response.StatusCode, string.Empty);
			}
			else
			{
                proxyResult = new ProxyResponse<NotificationOptions>(response.RawResponse, null, false, response.StatusCode, response.ReasonCode);
			}
			return proxyResult;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public ProxyResponse UpdateNotificationOptions(NotificationOptions options)
		{
			ProxyResponse proxyResult = null;

			ContentType = RequestContentType.ApplicationJson;
			OperationMethod = HttpMethod.Post;

			var uri = base.GetRequestUri(null);
			var response = GetResponse<NotificationOptions, ApiResponse>(uri, options);
			if (response.IsRequestSuccessfull)
			{
				proxyResult = new ProxyResponse(response.RawResponse, true, response.StatusCode, string.Empty);
			}
			else
			{
				proxyResult = new ProxyResponse(response.RawResponse, false, response.StatusCode, response.ReasonCode);
				proxyResult.MapApiResult(response.DataObject as ApiResponse);
			}
			return proxyResult;
		}
	}
}

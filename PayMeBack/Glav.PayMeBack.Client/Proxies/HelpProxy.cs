using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Client.Proxies
{
	public class HelpProxy : BaseProxy
	{
		public HelpProxy()
		{
			PagingEnabled = false;
		}

		public HelpProxy(string bearerToken) : base(bearerToken)
		{
			PagingEnabled = false;
		}

		public override string RequestPrefix
		{
			get { return ResourceNames.Help; }
		}

		public ProxyResponse<ApiHelp> GetHelp()
		{
			ProxyResponse<ApiHelp> proxyResult = null;

			OperationMethod = HttpMethod.Get;
			ContentType = RequestContentType.ApplicationJson;

			var uri = base.GetRequestUri(null);
			var response = GetResponse<ApiHelp>(uri);
			if (response.IsRequestSuccessfull)
			{
				proxyResult = new ProxyResponse<ApiHelp>(response.RawResponse,response.DataObject, true, response.StatusCode, string.Empty);
			}
			else
			{
				proxyResult = new ProxyResponse<ApiHelp>(response.RawResponse,null, false, response.StatusCode, response.ReasonCode);
			}
			return proxyResult;

		}
	}
}

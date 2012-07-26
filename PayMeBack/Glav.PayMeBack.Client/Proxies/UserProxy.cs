using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;
using System.Net.Http;

namespace Glav.PayMeBack.Client.Proxies
{
	public class UserProxy : BaseProxy
	{
		public UserProxy()
		{
			PagingEnabled = true;
		}

		public UserProxy(string bearerToken)
			: base(bearerToken)
		{
			PagingEnabled = true;
		}

		public override string RequestPrefix
		{
			get { return ResourceNames.User; }
		}

		public ProxyResponse<IEnumerable<User>> Search(string searchCriteria, RequestPagingFilter pagingFilter)
		{
			ProxyResponse<IEnumerable<User>> proxyResult = null;

			OperationMethod = HttpMethod.Get;
			ContentType = RequestContentType.ApplicationJson;

			var uri = base.GetRequestUri(string.Format("?searchCriteria={0}", searchCriteria), pagingFilter);
			var response = GetResponse<IEnumerable<User>>(uri);
			if (response.IsRequestSuccessfull)
			{
				proxyResult = new ProxyResponse<IEnumerable<User>>(response.RawResponse, response.DataObject, true, response.StatusCode, string.Empty);
			}
			else
			{
				proxyResult = new ProxyResponse<IEnumerable<User>>(response.RawResponse, null, false, response.StatusCode, response.ReasonCode);
			}
			return proxyResult;
		}

		public ProxyResponse<User> GetByEmail(string email)
		{
			ProxyResponse<User> proxyResult = null;

			OperationMethod = HttpMethod.Get;
			ContentType = RequestContentType.ApplicationJson;

			var uri = base.GetRequestUri(string.Format("?email={0}", email));
			var response = GetResponse<User>(uri);
			if (response.IsRequestSuccessfull)
			{
				proxyResult = new ProxyResponse<User>(response.RawResponse, response.DataObject, true, response.StatusCode, string.Empty);
			}
			else
			{
				proxyResult = new ProxyResponse<User>(response.RawResponse, null, false, response.StatusCode, response.ReasonCode);
			}
			return proxyResult;

		}
	}
}

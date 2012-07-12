using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Client.Proxies
{
	public class DebtProxy : BaseProxy
	{
		public DebtProxy()
			: base()
		{
			PagingEnabled = false;
		}

		public DebtProxy(string bearerToken)
			: base(bearerToken)
		{
			PagingEnabled = false;
		}

		public override string RequestPrefix
		{
			get { return ResourceNames.Debt; }
		}

		/// <summary>
		/// Service uses the current access token to retrieve the user so we dont need to
		/// pass it anything else like a user id
		/// </summary>
		/// <returns></returns>
		public ProxyResponse<UserPaymentPlan> GetDebtPaymentPlan()
		{
			ProxyResponse<UserPaymentPlan> proxyResult = null;

			ContentType = RequestContentType.ApplicationJson;
			var uri = base.GetRequestUri(null);
			var response = GetResponse<UserPaymentPlan>(uri);
			if (response.IsRequestSuccessfull)
			{
				proxyResult = new ProxyResponse<UserPaymentPlan>(response.RawResponse,response.DataObject,true,response.StatusCode,string.Empty);
			}
			else
			{
				proxyResult = new ProxyResponse<UserPaymentPlan>(response.RawResponse,null,false,response.StatusCode,response.ReasonCode);
			}
			return proxyResult;
		}
	}
}

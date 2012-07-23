using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
			OperationMethod = HttpMethod.Get;
			var uri = base.GetRequestUri(null);
			var response = GetResponse<UserPaymentPlan>(uri);
			if (response.IsRequestSuccessfull)
			{
				proxyResult = new ProxyResponse<UserPaymentPlan>(response.RawResponse, response.DataObject, true, response.StatusCode, string.Empty);
			}
			else
			{
				proxyResult = new ProxyResponse<UserPaymentPlan>(response.RawResponse, null, false, response.StatusCode, response.ReasonCode);
			}
			return proxyResult;
		}

		/// <summary>
		/// Service uses the current access token to retrieve the user so we dont need to
		/// pass it anything else like a user id
		/// </summary>
		/// <returns></returns>
		public ProxyResponse<ApiResponse> RemoveDebtFromPaymentPlan(Guid debtId)
		{
			ProxyResponse<ApiResponse> proxyResult = null;

			ContentType = RequestContentType.ApplicationJson;
			OperationMethod = HttpMethod.Delete;
			var uri = base.GetRequestUri(string.Format("?debtId={0}", debtId));
			var response = GetResponse<ApiResponse>(uri);
			proxyResult = new ProxyResponse<ApiResponse>(response.RawResponse, response.DataObject, response.IsRequestSuccessfull, response.StatusCode, string.Empty);
			return proxyResult;
		}

		/// <summary>
		/// Service uses the current access token to retrieve the user so we dont need to
		/// pass it anything else like a user id
		/// </summary>
		/// <returns></returns>
		public ProxyResponse UpdatePaymentPlan(UserPaymentPlan paymentPlan)
		{
			ProxyResponse proxyResult = null;

			ContentType = RequestContentType.ApplicationJson;
			OperationMethod = HttpMethod.Post;

			var uri = base.GetRequestUri(null);
			var response = GetResponse<UserPaymentPlan, ApiResponse>(uri, paymentPlan);
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


		public ProxyResponse<DebtSummary> GetDebtSummary()
		{
			ProxyResponse<DebtSummary> proxyResult = null;

			ContentType = RequestContentType.ApplicationJson;
			OperationMethod = HttpMethod.Get;

			var uri = base.GetRequestUri("Summary");
			var response = GetResponse<DebtSummary>(uri);
			if (response.IsRequestSuccessfull)
			{
				proxyResult = new ProxyResponse<DebtSummary>(response.RawResponse, response.DataObject, true, response.StatusCode, string.Empty);
			}
			else
			{
				proxyResult = new ProxyResponse<DebtSummary>(response.RawResponse, null, false, response.StatusCode, response.ReasonCode);
			}
			return proxyResult;
		}
	}
}

using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;
using System.Web.Script.Serialization;

using Glav.PayMeBack.Core;
using System.Net.Http.Formatting;

namespace Glav.PayMeBack.Client
{
	public abstract class BaseProxy
	{
		private string _baseUri;
		private string _bearerToken; // for OAuth

		public BaseProxy()
		{
			_baseUri = ProxyConfig.BaseUri;
			ContentType = RequestContentType.ApplicationJson;
			OperationMethod = HttpMethod.Get;
			PagingEnabled = true;
		}

		public BaseProxy(string bearerToken) : this()
		{
			_bearerToken = bearerToken;
		}

		public HttpMethod OperationMethod { get; set; }
		public bool PagingEnabled { get; set; }
		public string BearerToken { get { return _bearerToken; } set { _bearerToken = value; } }

		/// <summary>
		/// Indicates the prefix used to access the service in the Url.
		/// </summary>
		/// <example>The API call for getting a contact by Id might be 'http://somehost/Contacts/1' where the Id
		/// is 1. The request prefix in this case is 'Contacts' and is consistent for all calls relating to Contacts</example>
		public abstract string RequestPrefix { get;  }

		/// <summary>
		/// Indicates whether to request the response in Json, Xml etc..
		/// </summary>
		public RequestContentType ContentType { get; set; }

		private StringBuilder GetBaseUri(string apiMethod)
		{
			if (string.IsNullOrWhiteSpace(_baseUri))
			{
				throw new System.ArgumentNullException("Base Uri is null (not configured)");
			}
			var uri = new StringBuilder();
			uri.AppendFormat("{0}", _baseUri);
			if (!_baseUri.EndsWith("/"))
			{
				uri.Append("/");
			}
			if (!string.IsNullOrWhiteSpace(RequestPrefix))
			{
				uri.AppendFormat("{0}", RequestPrefix);
			}
			if (!string.IsNullOrWhiteSpace(apiMethod))
			{
				if (!uri.ToString().EndsWith("/"))
				{
					uri.Append("/");
				}
				uri.AppendFormat("{0}", apiMethod);
			}
			return uri;
		}

		protected virtual string GetRequestUri(string apiMethod)
		{
			return GetRequestUri(apiMethod, new RequestPagingFilter());
		}

		protected virtual string GetRequestUri(string apiMethod, HttpMethod httpMethod)
		{
			return GetRequestUri(apiMethod, null, httpMethod);
		}

		protected virtual string GetRequestUri(string apiMethod, RequestPagingFilter pagingFilter)
		{
			return GetRequestUri(apiMethod, pagingFilter, HttpMethod.Get);
		}

		protected virtual string GetRequestUri(string apiMethod, RequestPagingFilter pagingFilter, HttpMethod httpMethod)
		{
			if (pagingFilter == null)
			{
				pagingFilter = new RequestPagingFilter();
			}
			var uri = GetBaseUri(apiMethod);
			if (httpMethod == HttpMethod.Get && PagingEnabled)
			{
				uri.AppendFormat("&{0}={1}&{2}={3}", ApiConstants.PageQueryArg, pagingFilter.Page, ApiConstants.PageSizeQueryArg,
								 pagingFilter.PageSize);
			}

			return uri.ToString();
		}

		public virtual ProxyResponse GetResponse(string requestUrl)
		{
			var responseMsg = GetResponseMessage(requestUrl);
			var rawResponse = responseMsg.Content != null ? responseMsg.Content.ReadAsStringAsync().Result : string.Empty;
			return new ProxyResponse(rawResponse, responseMsg.IsSuccessStatusCode, responseMsg.StatusCode, responseMsg.ReasonPhrase);
		}

		public virtual ProxyResponse<T> GetResponse<T>(string requestUri) where T : class
		{
			return GetResponse<T>(requestUri, null);
		}

		public virtual ProxyResponse<T> GetResponse<T>(string requestUri, T postData) where T : class
		{
			HttpResponseMessage responseMsg;
			if (OperationMethod != HttpMethod.Put && OperationMethod != HttpMethod.Post)
			{
				responseMsg = GetResponseMessage<object>(requestUri, null);
			}
			else
			{
				responseMsg = GetResponseMessage<T>(requestUri,postData);
			}
			if (responseMsg.IsSuccessStatusCode)
			{
				if (responseMsg.Content != null)
				{
					var textData = responseMsg.Content.ReadAsStringAsync().Result;
					if (!string.IsNullOrWhiteSpace(textData))
					{
						var dto = Deserialise<T>(textData);
						return new ProxyResponse<T>(textData, dto, true, responseMsg.StatusCode, responseMsg.ReasonPhrase);
					}
					return new ProxyResponse<T>(textData, default(T), true, responseMsg.StatusCode, responseMsg.ReasonPhrase);
				}
			}

			return new ProxyResponse<T>(responseMsg.Content.ReadAsStringAsync().Result, default(T), false, responseMsg.StatusCode, responseMsg.ReasonPhrase);

		}

		protected virtual System.Net.Http.HttpResponseMessage GetResponseMessage(string requestUri)
		{
			return GetResponseMessage<object>(requestUri, null);
		}
		protected virtual System.Net.Http.HttpResponseMessage GetResponseMessage<T>(string requestUri, T postData)
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.AsContentTypeString()));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(OAuthTokenType.Bearer, _bearerToken);

			MediaTypeFormatter mediaFormatter;

			if (ContentType == RequestContentType.ApplicationXml)
			{
				mediaFormatter = new System.Net.Http.Formatting.XmlMediaTypeFormatter();

			} else
			{
				mediaFormatter = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
			}
			HttpResponseMessage responseMsg = null;
			if (OperationMethod == HttpMethod.Get)
			{
				responseMsg = client.GetAsync(requestUri).Result;
			}
			else if (OperationMethod == HttpMethod.Delete && postData == null)
			{
				responseMsg = client.DeleteAsync(requestUri).Result;
			}
			else if (OperationMethod == HttpMethod.Head)
			{
				var rqstMsg = new HttpRequestMessage(HttpMethod.Head, requestUri);
				responseMsg = client.SendAsync(rqstMsg).Result;
			}
			else
			{
				//Note: Need to explicitly specify the content type here otherwise this call fails.
				if (OperationMethod == HttpMethod.Put)
				{
					responseMsg = client.PutAsync<T>(requestUri, postData, mediaFormatter).Result;
				}
				else
				{
					responseMsg = client.PostAsync<T>(requestUri, postData, mediaFormatter).Result;
				}
			}

			return responseMsg;
		}

		public T Deserialise<T>(string data) where T : class
		{
			T dto;
			if (ContentType == RequestContentType.ApplicationJson)
			{
				JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
				dto = jsonSerializer.Deserialize<T>(data);
			}
			else
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				dto = xmlSerializer.Deserialize(new StringReader(data)) as T;
			}

			return dto;
		}


	}
}

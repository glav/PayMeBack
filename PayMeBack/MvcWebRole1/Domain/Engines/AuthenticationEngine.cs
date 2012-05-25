using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Glav.PayMeBack.Web.Domain.Services;

namespace Glav.PayMeBack.Web.Domain.Engines
{
	public class AuthenticationEngine : DelegatingHandler
	{
		private IOAuthSecurityService _oAuthSecurityService;

		public AuthenticationEngine(IOAuthSecurityService oAuthSecurityService) 
		{
			_oAuthSecurityService = oAuthSecurityService;
		}

		public AuthenticationEngine(HttpMessageHandler innerHandler, IOAuthSecurityService oAuthSecurityService)
			: base(innerHandler)
		{
			_oAuthSecurityService = oAuthSecurityService; 
		}

		protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			if (!request.IsOAuthExplicitGrantRequest())
			{
				// Is it a valid token/oAuth request being made?
				if (request.DoesContainOAuthToken())
				{
					var bearerToken = request.Headers.Authorization.Parameter;
					if (!_oAuthSecurityService.IsAccessTokenValid(bearerToken))
					{
						return Task.Factory.StartNew(() =>
						{
							var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
							return response;
						});

						// there was a valid token present
						return base.SendAsync(request, cancellationToken);
					}
				}
				// There was no authorisation request we could find in the path
				// and there was no token. So that means no access
				return Task.Factory.StartNew(() =>
				{
					var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
					return response;
				});

			}

			// If we get here, it is only because the access is going to an OAuth
			// specific endpoit such as http://host/authorisation
			// to get an access key, refresh token etc... this endpoint
			// must allow anonymous users
			return base.SendAsync(request, cancellationToken);
		}
	}
}

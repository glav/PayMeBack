using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Models;

namespace Glav.PayMeBack.Web.Controllers.Api
{
    public class SimpleSignInController : ApiController
    {
		private IOAuthSecurityService _securityService;

		public SimpleSignInController(IOAuthSecurityService securityService)
    	{
			_securityService = securityService;
    	}
		public SignInResponse PostSignInDetails(string email, string password)
		{
			var response = new SignInResponse();
			var user = _securityService.SignIn(email, password);
			if (user != null)
			{
				response.IsSuccessful = true;
				response.AccessToken = user.Id;
			}
			return response;
		}
    }
}

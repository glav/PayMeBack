using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Glav.PayMeBack.Web.Models;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Web.Controllers.Api
{
    public class SignUpController : ApiController
    {
    	private ISignupService _signupService;

    	public SignUpController(ISignupService signupService)
    	{
    		_signupService = signupService;
    	}

		public object PostSignUpDetails(string emailAddress, string firstNames, string lastName, string password)
		{
			var response = new OAuthAuthorisationGrantResponse();
			try
			{
				response = _signupService.SignUpNewUser(emailAddress, firstNames, lastName, password);
				if (response.IsSuccessfull)
				{
					return response.AccessGrant;
				}

				return response.ErrorDetails;
			}
			catch (Exception ex)
			{
				response.IsSuccessfull = false;
				response.ErrorDetails = new OAuthGrantRequestError { error = "invalid_client" };
				return response.ErrorDetails;
			}
		}
    }
}

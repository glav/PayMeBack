using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Glav.PayMeBack.Web.Models;
using Glav.PayMeBack.Web.Domain.Services;

namespace Glav.PayMeBack.Web.Controllers.Api
{
    public class SignUpController : ApiController
    {
    	private ISignupService _signupService;

    	public SignUpController(ISignupService signupService)
    	{
    		_signupService = signupService;
    	}

		public SignUpResponse PostSignUpDetails(string emailAddress, string firstNames, string lastName, string password)
		{
			var response = new SignUpResponse();
			try
			{
				response.AccessToken = _signupService.SignUpNewUser(emailAddress, firstNames, lastName, password);
				response.IsSuccessful = true;
			}
			catch (Exception ex)
			{
				response.ErrorMessage = ex.Message;
			}
			return response;
		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Glav.PayMeBack.Web.Models;

namespace Glav.PayMeBack.Web.Controllers.Api
{
    public class SignUpController : ApiController
    {
		public ApiResponse PostSignUpDetails(string emailAddress, string firstNames, string lastName, string password)
		{
			throw new NotImplementedException();
		}
    }
}

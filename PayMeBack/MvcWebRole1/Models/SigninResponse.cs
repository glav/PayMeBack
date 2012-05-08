using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Models
{
	public class SignInResponse : ApiResponse
	{
		public User User { get; set; }
	}
}
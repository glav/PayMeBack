using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class SecurityService : ISecurityService
	{
		public string CreateHashValue(string input)
		{
			//TODO:return a proper hash value
			return input;
		}
	}
}
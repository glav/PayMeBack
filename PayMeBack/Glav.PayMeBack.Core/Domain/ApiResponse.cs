using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Core.Domain
{
	public class ApiResponse
	{
		public ApiResponse()
		{
			ErrorMessages = new List<string>();
		}
		public bool IsSuccessful { get; set; }
		public List<string> ErrorMessages { get; set; }
	}
}
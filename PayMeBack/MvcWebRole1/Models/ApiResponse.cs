﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Models
{
	public class ApiResponse
	{
		public bool IsSuccessful { get; set; }
		public string ErrorMessage { get; set; }
	}
}
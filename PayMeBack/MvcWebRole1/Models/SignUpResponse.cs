﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Models
{
	public class SignUpResponse : ApiResponse
	{
		public Guid UserId { get; set; }
	}
}
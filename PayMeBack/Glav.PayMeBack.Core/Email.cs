using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Glav.PayMeBack.Core
{
	public class EmailValidator
	{
		public const string ValidationRegex = @"^[A-Z0-9._\-\+!#\$%&'\*\+/=\?\^`\{\}\|~\w]+@[A-Z0-9._\-\+]+\.[A-Z]{2,6}$";  // good

		public bool IsValid(string emailAddress)
		{
			if (string.IsNullOrWhiteSpace(emailAddress))
			{
				return false;
			}

			var trimmedEmail = emailAddress.Trim();

			if (trimmedEmail.Length > 127)
			{
				return false;
			}

			var regex = new Regex(ValidationRegex, RegexOptions.IgnoreCase);
			if (!regex.IsMatch(trimmedEmail))
			{
				return false;
			}

			return true;
		}

	}
}

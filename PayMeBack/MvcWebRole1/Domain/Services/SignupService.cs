﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class SignupService : ISignupService
	{
		private IEmailService _emailService;
		private IUserService _userService;

		public SignupService(IEmailService emailService, IUserService userService)
		{
			_emailService = emailService;
			_userService = userService;
		}
		public Guid SignUpNewUser(string emailAddress, string firstNames, string lastName, string password)
		{
			IsEmailValid(emailAddress);
			VerifyUserDoesNotExist(emailAddress);
			return _userService.RegisterUser(emailAddress, firstNames, lastName, password);
		}

		private void VerifyUserDoesNotExist(string emailAddress)
		{
			var user = _userService.GetUser(emailAddress);
			if (user != null)
			{
				throw new Exception(string.Format("User [{0}] already exists", emailAddress));
			}
		}

		private void IsEmailValid(string emailAddress)
		{
			if (!_emailService.IsValidEmail(emailAddress))
			{
				throw new ArgumentException("Invalid Email Address",emailAddress);
			}
		}

	}
}
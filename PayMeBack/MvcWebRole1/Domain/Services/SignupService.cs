using System;
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
		private ISecurityService _securityService;

		public SignupService(IEmailService emailService, IUserService userService, ISecurityService securityService)
		{
			_emailService = emailService;
			_userService = userService;
			_securityService = securityService;
		}
		public Guid SignUpNewUser(string emailAddress, string firstNames, string lastName, string password)
		{
			IsEmailValid(emailAddress);
			VerifyUserDoesNotExist(emailAddress);
			var registeredUser= RegisterUser(emailAddress, firstNames, lastName, password);
			return registeredUser.Id;
		}

		private User RegisterUser(string emailAddress, string firstNames, string lastName, string password)
		{
			var user = new User {EmailAddress = emailAddress, FirstNames = firstNames, Surname = lastName,Password = _securityService.CreateHashValue(password)};
			_userService.RegisterUser(user);
			return user;
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
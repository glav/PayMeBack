using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Data;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Web.Domain.Engines;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class SignupManager : ISignupManager
	{
		private IEmailEngine _emailService;
		private IUserEngine _userService;
		private IOAuthSecurityService _securityService;

		public SignupManager(IEmailEngine emailService,IUserEngine userService, IOAuthSecurityService securityService)
		{
			_emailService = emailService;
			_userService = userService;
			_securityService = securityService;
		}
		public OAuthAuthorisationGrantResponse SignUpNewUser(string emailAddress, string firstNames, string lastName, string password)
		{
			IsEmailValid(emailAddress);
			VerifyUserDoesNotExist(emailAddress);

			var user = new User { EmailAddress = emailAddress, FirstNames = firstNames, Surname = lastName };

			_userService.SaveOrUpdateUser(user, password);
			_userService.SetUserToValidated(user.Id);
			var scope = new AuthorisationScope() { ScopeType = AuthorisationScopeType.Readonly };
			return _securityService.AuthorisePasswordCredentialsGrant(emailAddress, password, scope.ToTextValue());

		}

		/// <summary>
		/// Sets a user to be validated. Validated users can sign in and create payment
		/// plans. Non validated users are attached to debts but are not necessarily users
		/// in the system.
		/// </summary>
		/// <param name="userId"></param>
		public void SetUserToValidated(Guid userId)
		{
			_userService.SetUserToValidated(userId);
		}


		private void VerifyUserDoesNotExist(string emailAddress)
		{
			var user = _userService.GetUserByEmail(emailAddress);
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
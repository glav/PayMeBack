using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class SignInService : ISignInService
	{
		private IRepository _repository;
		private ISecurityService _securityService;

		public SignInService(IRepository repository,ISecurityService securityService)
		{
			_repository = repository;
			_securityService = securityService;
		}
		public User SignIn(string email, string password)
		{
			var user = _repository.GetUser(email);
			if (_securityService.CreateHashValue(user.Password) == _securityService.CreateHashValue(password))
			{
				return user;
			}
			return null;
		}
	}
}
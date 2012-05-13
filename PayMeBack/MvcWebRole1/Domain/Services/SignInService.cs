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
			var currentPwd = _repository.GetUserPassword(email);
			if (_securityService.CreateHashValue(currentPwd) == _securityService.CreateHashValue(password))
			{
				return _repository.GetUser(email);
			}
			return null;
		}
	}
}
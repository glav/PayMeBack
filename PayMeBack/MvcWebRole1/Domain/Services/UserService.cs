using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain.Services
{
	/// <summary>
	/// This class looks suspiciously like a CRUD class but it deals with caching
	/// as well and acts as the unit of work for saving/loading users.
	/// </summary>
	public class UserService : IUserService
	{
		private IRepository _repository;
		private ISecurityService _securityService;

		public UserService(IRepository repository, ISecurityService securityService)
		{
			_repository = repository;
			_securityService = securityService;
		
		}
		public User GetUser(string emailAddress)
		{
			//TODO: Use cache

			var user = _repository.GetUser(emailAddress);
			return user;
		}

		public void SaveOrUpdateUser(User user)
		{
			throw new NotImplementedException();
		}

		public void DeleteUser(User user)
		{
			throw new NotImplementedException();
		}

		public Guid RegisterUser(string emailAddress, string firstNames, string lastName, string password)
		{
			// TODO: encrypt/hash pwd
			// TODO: save to DB with new Guid as ID
			var existingUser = _repository.GetUser(emailAddress);
			if ( existingUser != null)
			{
				throw new Exception(string.Format("User {0} already exists.",emailAddress));
			}

			var user = new User {EmailAddress = emailAddress, FirstNames = firstNames, Surname = lastName};
			return _repository.AddUser(user,password);
		}
	}
}
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

		public UserService(IRepository repository)
		{
			_repository = repository;
		
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

		public void RegisterUser(User user, string password)
		{
			// TODO: encrypt/hash pwd
			// TODO: save to DB with new Guid as ID

			if (_repository.GetUser(user.EmailAddress) != null)
			{
				throw new Exception(string.Format("User {0} already exists.",user.EmailAddress));
			}

			// dummy for now
			user.Id = Guid.NewGuid();
		}
	}
}
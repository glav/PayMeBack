using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using Glav.PayMeBack.Web.Data;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Web.Domain.Services
{
	/// <summary>
	/// This class looks suspiciously like a CRUD class but it deals with caching
	/// as well and acts as the unit of work for saving/loading users.
	/// </summary>
	public class UserService : IUserService
	{
		private ICrudRepository _crudRepository;
		private IOAuthSecurityService _securityService;

		public UserService(ICrudRepository crudRepository, IOAuthSecurityService securityService)
		{
			_crudRepository = crudRepository;
			_securityService = securityService;
		
		}
		public User GetUserByEmail(string emailAddress)
		{
			//TODO: Use cache

			var userDetail = _crudRepository.GetSingle<UserDetail>(u => u.EmailAddress == emailAddress);
			return new User(userDetail);
		}

		public User GetUserById(Guid id)
		{
			//TODO: Use cache

			var userDetail = _crudRepository.GetSingle<UserDetail>(u => u.Id == id);
			return new User(userDetail);
		}

		public void SaveOrUpdateUser(User user, string password = null)
		{
			var currentUser = _crudRepository.GetSingle<UserDetail>(u => u.Id == user.Id);
			if (currentUser != null)
			{
				currentUser = new UserDetail();
				MapUserToUserDetail(user,currentUser,password);
				_crudRepository.Insert<UserDetail>(currentUser);
				user.Id = currentUser.Id;
				return;
			}
			MapUserToUserDetail(user,currentUser,password);
			_crudRepository.Update<UserDetail>(currentUser);
		}

		private void MapUserToUserDetail(User user, UserDetail userDetail, string password = null)
		{
			userDetail.EmailAddress = user.EmailAddress;
			userDetail.FirstNames = user.FirstNames;
			userDetail.Surname = user.Surname;
			if (!string.IsNullOrEmpty(password))
			{
				userDetail.Password = _securityService.CreateHashedTokenFromInput(password);
			}
		}

		public void DeleteUser(User user)
		{
			_crudRepository.Delete<UserDetail>(u => u.Id == user.Id);
		}

		public OAuthAuthorisationGrantResponse RegisterUser(string emailAddress, string firstNames, string lastName, string password)
		{
			// TODO: encrypt/hash pwd
			// TODO: save to DB with new Guid as ID
			var existingUser = GetUserByEmail(emailAddress);
			if ( existingUser != null)
			{
				throw new Exception(string.Format("User {0} already exists.",emailAddress));
			}

			var user = new User {EmailAddress = emailAddress, FirstNames = firstNames, Surname = lastName};

			SaveOrUpdateUser(user, password);
			var scope = new AuthorisationScope() { ScopeType = AuthorisationScopeType.Readonly};
			return _securityService.AuthorisePasswordCredentialsGrant(emailAddress, password, scope.ToString());
		}
	}
}
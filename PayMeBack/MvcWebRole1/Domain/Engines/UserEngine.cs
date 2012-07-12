using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Data;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Web.Domain.Services;

namespace Glav.PayMeBack.Web.Domain.Engines
{
	/// <summary>
	/// This class looks suspiciously like a CRUD class but it deals with caching
	/// as well and acts as the unit of work for saving/loading users.
	/// </summary>
	public class UserEngine : IUserEngine
	{
		private ICrudRepository _crudRepository;
		private IOAuthSecurityService _securityService;

		public UserEngine(ICrudRepository crudRepository, IOAuthSecurityService securityService)
		{
			_crudRepository = crudRepository;
			_securityService = securityService;
		
		}
		public User GetUserByEmail(string emailAddress)
		{
			//TODO: Use cache

			var userDetail = _crudRepository.GetSingle<UserDetail>(u => u.EmailAddress == emailAddress);
			if (userDetail == null)
			{
				return null;
			}
			return userDetail.ToModel();
		}

		public User GetUserById(Guid id)
		{
			//TODO: Use cache

			var userDetail = _crudRepository.GetSingle<UserDetail>(u => u.Id == id);
			return userDetail.ToModel();
		}

		public void SaveOrUpdateUser(User user, string password = null)
		{
			var currentUser = _crudRepository.GetSingle<UserDetail>(u => u.Id == user.Id);
			if (currentUser == null)
			{
				currentUser = new UserDetail();
				currentUser.Id = Guid.NewGuid();
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

	}
}
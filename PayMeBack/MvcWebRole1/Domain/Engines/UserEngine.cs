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
		private IUserRepository _userRepository;

		public UserEngine(ICrudRepository crudRepository, IOAuthSecurityService securityService, IUserRepository userRepository)
		{
			_crudRepository = crudRepository;
			_securityService = securityService;
			_userRepository = userRepository;

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

		public User GetUserByAccessToken(string token)
		{
			//TODO: Use cache

			var tokenRecord = _crudRepository.GetSingle<OAuthToken>(t => t.AccessToken == token);
			if (token == null)
			{
				return null;
			}
			if (!_securityService.IsAccessTokenValid(token))
			{
				throw new SecurityException("Access token not valid");
			}
			var userDetail = _crudRepository.GetSingle<UserDetail>(u => u.Id == tokenRecord.AssociatedUserId);
            if (userDetail != null)
            {
                return userDetail.ToModel();
            }
		    return null;
		}

		private UserDetail ValidateUserForSave(User user)
		{
			if (user == null)
			{
				throw new ArgumentException("No user data to save.");
			}

			if (string.IsNullOrWhiteSpace(user.EmailAddress))
			{
				throw new ArgumentException("Email cannot be empty");
			}

            var emailValidator = new EmailValidator();
            if (!emailValidator.IsValid(user.EmailAddress))
            {
                throw new ArgumentException("Invalid Email Address", user.EmailAddress);
            }

            UserDetail currentUser = null;
			if (user.Id != Guid.Empty)
			{
				currentUser = _crudRepository.GetSingle<UserDetail>(u => u.Id == user.Id);
			}
			else
			{
				// Id is empty so it is probably a new user BUT we need to ensure we
				// dont save the user with an email that already exists
				var normalisedEmailAddress = user.EmailAddress.ToLowerInvariant();
				var existingUser = _crudRepository.GetSingle<UserDetail>(u => u.EmailAddress == normalisedEmailAddress);
				if (existingUser != null)
				{
					throw new ArgumentException("User with that email already exists and a new one cannot be added");
				}
			}
			return currentUser;

		}
		public void SaveOrUpdateUser(User user, string password = null)
		{
			var currentUser = ValidateUserForSave(user);
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
			userDetail.EmailAddress = user.EmailAddress.ToLowerInvariant();
			userDetail.FirstNames = user.FirstNames;
			userDetail.Surname = user.Surname;
			if (!string.IsNullOrEmpty(password))
			{
				userDetail.Password = _securityService.CreateHashedTokenFromInput(password);
			}
		}

		public void SetUserToValidated(Guid userId)
		{
			var user = _crudRepository.GetSingle<UserDetail>(u => u.Id == userId && !u.IsValidated);
			if (user != null)
			{
				user.IsValidated = true;
				_crudRepository.Update<UserDetail>(user);
			}
		}

		public void DeleteUser(User user)
		{
			_crudRepository.Delete<UserDetail>(u => u.Id == user.Id);
		}

		public IEnumerable<User> SearchUsers(RequestPagingFilter pagingFilter, string searchCriteria)
		{
			var userList = new List<User>();

			if (pagingFilter == null)
			{
				pagingFilter = new RequestPagingFilter();
			}
			if (string.IsNullOrWhiteSpace(searchCriteria))
			{
				var allUsers = _crudRepository.GetAll<UserDetail>(u => true, pagingFilter.Page, pagingFilter.PageSize, query => query.OrderBy(o => o.EmailAddress));
			
				allUsers.ToList().ForEach(u => userList.Add(u.ToModel()));
				return userList;
			}

			var normalisedCriteria = searchCriteria.ToLowerInvariant();
			var users = _userRepository.Search(pagingFilter,normalisedCriteria);
			users.ToList().ForEach(u => userList.Add(u.ToModel()));
			return userList;

		}
	}
}
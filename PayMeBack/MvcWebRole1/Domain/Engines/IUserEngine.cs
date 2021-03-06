﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Web.Domain.Engines
{
	public interface IUserEngine
	{
		User GetUserByEmail(string emailAddress);
		User GetUserById(Guid id);
		void SaveOrUpdateUser(User user, string password = null);
		void DeleteUser(User user);
		User GetUserByAccessToken(string token);
		void SetUserToValidated(Guid userId);
		IEnumerable<User> SearchUsers(RequestPagingFilter pagingFilter,string searchCriteria);
	}
}
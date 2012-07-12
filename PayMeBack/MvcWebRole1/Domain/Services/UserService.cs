using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class UserService : IUserService
	{
		public IEnumerable<User> GetAllUsersConnectedToUser(Core.RequestPagingFilter paging, Guid userId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<User> FindByEmail(Core.RequestPagingFilter paging, string emailCriteria)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<User> FindByName(Core.RequestPagingFilter paging, string nameCriteria)
		{
			throw new NotImplementedException();
		}
	}
}
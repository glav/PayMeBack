using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface IUserService
	{
		IEnumerable<User> GetAllUsersConnectedToUser(RequestPagingFilter paging, Guid userId);
		IEnumerable<User> FindByEmail(RequestPagingFilter paging, string emailCriteria);
		IEnumerable<User> FindByName(RequestPagingFilter paging, string nameCriteria);
	}
}
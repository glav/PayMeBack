using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Data
{
	public class UserRepository :IUserRepository
	{
		public IEnumerable<UserDetail> Search(Core.RequestPagingFilter paging, string searchCriteria)
		{
			using (var context = new PayMeBackEntities())
			{
				var results = context.SearchUsers(searchCriteria, paging.Page, paging.PageSize);
				return results.ToList();
			}
		}
	}
}
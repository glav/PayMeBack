using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Web.Data
{
	public interface IUserRepository
	{
		IEnumerable<UserDetail> Search(RequestPagingFilter paging, string searchCriteria);
	}
}
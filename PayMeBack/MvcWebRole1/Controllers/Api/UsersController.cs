using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Domain.Engines;

namespace Glav.PayMeBack.Web.Controllers.Api
{
	/// <summary>
	/// Performs basic user operations such as retrieval and search
	/// </summary>
    public class UsersController : ApiController
	{
		private IUserEngine _userEngine;

		public UsersController(IUserEngine userEngine)
		{
			_userEngine = userEngine;
		}

		/// <summary>
		/// Search for users via any text criteria with paging support.
		/// </summary>
		/// <param name="pagingFilter"></param>
		/// <param name="searchCriteria"></param>
		/// <returns></returns>
		public IEnumerable<User> GetSearch([FromUri] RequestPagingFilter pagingFilter, string searchCriteria)
		{
			return _userEngine.SearchUsers(pagingFilter, searchCriteria);
		}
    }
}

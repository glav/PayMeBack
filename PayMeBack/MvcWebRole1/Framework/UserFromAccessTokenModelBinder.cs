using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.PayMeBack.Web.Domain.Services;

namespace Glav.PayMeBack.Web.Framework
{
	public class UserFromAccessTokenModelBinder : System.Web.Http.ModelBinding.IModelBinder
	{
		private IUserEngine _userEngine;

		public UserFromAccessTokenModelBinder(IUserEngine userEngine)
		{
			_userEngine = userEngine;
		}

		public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, System.Web.Http.ModelBinding.ModelBindingContext bindingContext)
		{
			var accessToken = actionContext.Request.Headers.Authorization.Parameter;
			if (string.IsNullOrWhiteSpace(accessToken))
			{
				return false;
			}

			bindingContext.Model = _userEngine.GetUserByAccessToken(accessToken);
			return true;
		}
	}
}
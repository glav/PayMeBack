﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Web.Framework
{
	public class PayMeBackModelBinderProvider : ModelBinderProvider
	{
        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
		{
			if (modelType == typeof(User))
			{
				var resolver = GlobalConfiguration.Configuration.DependencyResolver;
				var binder = resolver.GetService(typeof (UserFromAccessTokenModelBinder));
				return binder as IModelBinder;
			}
			return null;
		}

    }
}
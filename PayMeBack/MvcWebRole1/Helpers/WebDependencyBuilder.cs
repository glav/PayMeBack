using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Glav.PayMeBack.Web.Data;
using Glav.CacheAdapter.Core.DependencyInjection;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Controllers.Api;
using Glav.CacheAdapter.Core;
using Glav.PayMeBack.Web.Framework;
using System.Web.Http.ModelBinding;

namespace Glav.PayMeBack.Web.Helpers
{
	public class WebDependencyBuilder
	{
		public IContainer BuildDependencies()
		{
			var builder = new ContainerBuilder();

			builder.Register(c => new CrudRepository()).As<ICrudRepository>();
			builder.Register(c => CacheBinder.ResolveCacheFromConfig(null)).As<ICacheProvider>().SingleInstance();

			builder.Register(c => new EmailEngine()).As<IEmailEngine>();
			builder.Register(c => new UserEngine(c.Resolve<ICrudRepository>(), c.Resolve<IOAuthSecurityService>())).As<IUserEngine>();
			builder.Register(c => new OAuthSecurityService(c.Resolve<ICrudRepository>(), c.Resolve<ICacheProvider>())).As<IOAuthSecurityService>();
			builder.Register(c => new SignupManager(c.Resolve<IEmailEngine>(), c.Resolve<IUserEngine>(), c.Resolve<IOAuthSecurityService>())).As<ISignupManager>();
			builder.Register(c => new ApiUsageLoggerEngine(c.Resolve<ICrudRepository>())).As<IUsageLogger>();
			builder.Register(c => new DebtRepository()).As<IDebtRepository>();
			builder.Register(c => new PaymentPlanService(c.Resolve<IUserEngine>(), c.Resolve<ICrudRepository>(), c.Resolve<Data.IDebtRepository>(), c.Resolve<ICacheProvider>())).As<IPaymentPlanService>();
			builder.Register(c => new AuthenticationEngine(c.Resolve<IOAuthSecurityService>(), c.Resolve<IUsageLogger>())).As<AuthenticationEngine>();
			builder.Register(c => new ApiUsageLoggerEngine(c.Resolve<ICrudRepository>())).As<IUsageLogger>();
			builder.Register(c => new ApiUsageHandler(c.Resolve<IUsageLogger>())).As<ApiUsageHandler>();

			builder.Register(c => new PayMeBackModelBinderProvider()).As<System.Web.Http.ModelBinding.ModelBinderProvider>();
			builder.Register(c => new UserFromAccessTokenModelBinder(c.Resolve<IUserEngine>())).As<UserFromAccessTokenModelBinder>();

			// Register our controllers
			builder.Register(c => new OAuthController(c.Resolve<IOAuthSecurityService>(), c.Resolve<ISignupManager>())).As<OAuthController>();
			builder.Register(c => new DebtsController(c.Resolve<IPaymentPlanService>(), c.Resolve<IUserEngine>())).As<DebtsController>();
			return builder.Build();
		}
	}
}
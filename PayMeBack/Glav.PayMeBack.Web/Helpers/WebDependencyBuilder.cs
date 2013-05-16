﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Glav.PayMeBack.Web.Controllers;
using Glav.PayMeBack.Web.Data;
using Glav.CacheAdapter.Core.DependencyInjection;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Controllers.Api;
using Glav.CacheAdapter.Core;
using Glav.PayMeBack.Web.Framework;
using System.Web.Http.ModelBinding;
using Glav.PayMeBack.Web.Domain;
using Api = Glav.PayMeBack.Web.Controllers.Api;

namespace Glav.PayMeBack.Web.Helpers
{
	public class WebDependencyBuilder
	{
		public IContainer BuildDependencies()
		{
			var builder = new ContainerBuilder();

			builder.Register(c => new CrudRepository()).As<ICrudRepository>();
			builder.Register(c => CacheBinder.ResolveCacheFromConfig(null)).As<ICacheProvider>().SingleInstance();

			builder.Register(c => new UserRepository()).As<IUserRepository>();
            builder.Register(c => new CultureFormattingEngine()).As<ICultureFormattingEngine>();
			builder.Register(c => new UserEngine(c.Resolve<ICrudRepository>(), c.Resolve<IOAuthSecurityService>(),c.Resolve<IUserRepository>(),c.Resolve<ICacheProvider>())).As<IUserEngine>();
			builder.Register(c => new OAuthSecurityService(c.Resolve<ICrudRepository>(), c.Resolve<ICacheProvider>())).As<IOAuthSecurityService>();
            builder.Register(c => new AuthorisationEngine()).As<IAuthorisationEngine>();
            builder.Register(c => new SignupManager(c.Resolve<IUserEngine>(), c.Resolve<IOAuthSecurityService>())).As<ISignupManager>();
			builder.Register(c => new ApiUsageLoggerEngine(c.Resolve<ICrudRepository>())).As<IUsageLogger>();
			builder.Register(c => new DebtRepository()).As<IDebtRepository>();
			builder.Register(c => new PaymentPlanService(c.Resolve<IUserEngine>(), c.Resolve<ICrudRepository>(), 
                                    c.Resolve<Data.IDebtRepository>(), c.Resolve<ICacheProvider>(),
                                    c.Resolve<ICultureFormattingEngine>(),c.Resolve<IAuthorisationEngine>(),c.Resolve<IPaymentPlanEngine>())).As<IPaymentPlanService>();
			builder.Register(c => new AuthenticationEngine(c.Resolve<IOAuthSecurityService>(), c.Resolve<IUsageLogger>())).As<AuthenticationEngine>();
			builder.Register(c => new ApiUsageLoggerEngine(c.Resolve<ICrudRepository>())).As<IUsageLogger>();
			builder.Register(c => new ApiUsageHandler(c.Resolve<IUsageLogger>())).As<ApiUsageHandler>();
            builder.Register(c => new PaymentPlanEngine(c.Resolve<ICacheProvider>(), c.Resolve<IDebtRepository>(),c.Resolve<ICrudRepository>())).As<IPaymentPlanEngine>();
			builder.Register(c => new HelpEngine()).As<IHelpEngine>();
			builder.Register(c => new PayMeBackModelBinderProvider()).As<System.Web.Http.ModelBinding.ModelBinderProvider>();
			builder.Register(c => new UserFromAccessTokenModelBinder(c.Resolve<IUserEngine>())).As<UserFromAccessTokenModelBinder>();
            builder.Register(c => new NotificationService(c.Resolve<ICrudRepository>(),c.Resolve<IPaymentPlanEngine>(),c.Resolve<IAuthorisationEngine>())).As<INotificationService>();
			// Register Web specific Managers
			builder.Register(c => new WebMembershipManager(c.Resolve<IOAuthSecurityService>(), c.Resolve<ISignupManager>(),c.Resolve<IUserEngine>())).As<IWebMembershipManager>();
			
			// Register our controllers
			builder.Register(c => new Api.OAuthController(c.Resolve<IOAuthSecurityService>(), c.Resolve<ISignupManager>())).As<OAuthController>();
			builder.Register(c => new Api.DebtsController(c.Resolve<IPaymentPlanService>())).As<DebtsController>();
			builder.Register(c => new Api.HelpController(c.Resolve<IHelpEngine>())).As<HelpController>();
			builder.Register(c => new DocumentationController(c.Resolve<IHelpEngine>())).As<DocumentationController>();
			builder.Register(c => new Api.UsersController(c.Resolve<IUserEngine>())).As<UsersController>();
            builder.Register(c => new Api.SummaryController(c.Resolve<IPaymentPlanService>())).As<Api.SummaryController>();
            builder.Register(c => new Api.PaymentController(c.Resolve<IPaymentPlanService>()));
            builder.Register(c => new Api.NotificationController(c.Resolve<INotificationService>()));

			builder.RegisterControllers(Assembly.GetExecutingAssembly());

			return builder.Build();
		}
	}
}
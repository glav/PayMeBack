using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using Glav.CacheAdapter.Core.DependencyInjection;
using Glav.PayMeBack.Core;
using Autofac;
using Glav.PayMeBack.Web.Domain.Services;
using Glav.PayMeBack.Web.Data;
using Glav.CacheAdapter.Core;
using System.Collections;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.PayMeBack.Web.Controllers.Api;

namespace Glav.PayMeBack.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{resource}.ico");

			// Our special OAuth authenticatio mechansism, base route == http://host/authorisation

			routes.MapHttpRoute(
				name: "SignUp",
				routeTemplate: ResourceNames.Authorisation+"/Signup",
				defaults: new { controller="OAuth", action="Signup"}
			);

			routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			SetupDepedencyInjection();
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterApis(GlobalConfiguration.Configuration);
			RegisterRoutes(RouteTable.Routes);

			BundleTable.Bundles.RegisterTemplateBundles();
		}

		private void SetupDepedencyInjection()
		{
			var config = GlobalConfiguration.Configuration;
			var builder = new ContainerBuilder();
			var defaultSvcResolver = new System.Web.Http.Services.DependencyResolver(config);

			builder.Register(c => new CrudRepository()).As<ICrudRepository>();
			builder.Register(c => CacheBinder.ResolveCacheFromConfig(null)).As<ICacheProvider>().SingleInstance();

			builder.Register(c => new EmailEngine()).As<IEmailEngine>();
			builder.Register(c => new UserEngine(c.Resolve<ICrudRepository>(), c.Resolve<IOAuthSecurityService>())).As<IUserEngine>();
			builder.Register(c => new OAuthSecurityService(c.Resolve<ICrudRepository>(), c.Resolve<ICacheProvider>())).As<IOAuthSecurityService>();
			builder.Register(c => new SignupManager(c.Resolve<IEmailEngine>(), c.Resolve<IUserEngine>(),c.Resolve<IOAuthSecurityService>())).As<ISignupManager>();
			builder.Register(c => new OAuthController(c.Resolve<IOAuthSecurityService>(), c.Resolve<ISignupManager>())).As<OAuthController>();
			builder.Register(c => new DebtRepository()).As<IDebtRepository>();
			builder.Register(c => new PaymentPlanService(c.Resolve<IUserEngine>(), c.Resolve<ICrudRepository>(),c.Resolve<Data.IDebtRepository>())).As<IPaymentPlanService>();
			
			var container = builder.Build();

			config.ServiceResolver.SetResolver(
				t =>
				{
					object o;
					if (container.TryResolve(t, out o))
					{
						return o;
					}
					else return defaultSvcResolver.GetService(t);// null;// defaultSvcResolver.GetService(t);
				},
				t =>
				{
					Type enumerableType = typeof(IEnumerable<>).MakeGenericType(new Type[] { t });
					var customTypes = ((IEnumerable)container.Resolve(enumerableType)).Cast<object>();
					var inbuiltTypes = defaultSvcResolver.GetServices(t);
					var types = new List<object>();
					types.AddRange(inbuiltTypes);
					types.AddRange(customTypes);

					return types;
				}
				);
		}

		private void RegisterApis(HttpConfiguration config)
		{
			config.MessageHandlers.Add(new AuthenticationEngine((IOAuthSecurityService)config.ServiceResolver.GetService(typeof(IOAuthSecurityService))));
			//config.MessageHandlers.Add(new ApiUsageLogger(container.Resolve<IUsageLogRepository>()));

			config.Routes.MapHttpRoute("OAuthPing", ResourceNames.Authorisation + "/{action}", new { controller = "OAuth" });
			config.Routes.MapHttpRoute("OAuth", ResourceNames.Authorisation, new { controller = "OAuth" });
		}

		private void RegisterMobileViews()
		{
			DisplayModeProvider.Instance.Modes.Insert(0, new
			DefaultDisplayMode("Mobile")
			{
				ContextCondition = (context =>
				{
					var userAgent = context.GetOverriddenUserAgent();
					var useMobileView = (userAgent.IndexOf("Mobi", StringComparison.OrdinalIgnoreCase) >= 0) || (userAgent.IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) >= 0);

					// This is for debugging with Firefox - comment out for real use
					//if (!useMobileView)
					//{
					//    useMobileView = userAgent.IndexOf("Mozilla", StringComparison.OrdinalIgnoreCase) >= 0;
					//}

					context.Items[MobileViewHelper.MobileContextKey] = useMobileView;
					return useMobileView;
				})
			});

		}

	}
}
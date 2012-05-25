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
			builder.Register(c => new OAuthSecurityService(c.Resolve<ICrudRepository>(),c.Resolve<ICacheProvider>());

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
			//config.MessageHandlers.Add(new AuthenticationEngine(container.Resolve<IApiAuthenticationService>(), container.Resolve<IOAuthSecurityService>()));
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
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
using Glav.PayMeBack.Web.Helpers;
using Glav.PayMeBack.Web.Domain;
using System.Net.Http;
using Glav.PayMeBack.Web.Framework;

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
			//var defaultSvcResolver = new System.Web.Http.Services.DependencyResolver(config);

			var builder = new WebDependencyBuilder();
			var container = builder.BuildDependencies();

			var resolver = new ApiDependencyResolver(container);
			GlobalConfiguration.Configuration.DependencyResolver = resolver;

		}

		private void RegisterApis(HttpConfiguration config)
		{
			var authHandler = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(AuthenticationEngine)) as DelegatingHandler;
			var usageHandler = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ApiUsageHandler)) as DelegatingHandler;

			config.MessageHandlers.Add(usageHandler);
			config.MessageHandlers.Add(authHandler);

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